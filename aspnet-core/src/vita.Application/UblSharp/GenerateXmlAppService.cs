using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using UblSharp;
using UblSharp.CommonAggregateComponents;
using UblSharp.CommonExtensionComponents;
using UblSharp.UnqualifiedDataTypes;
using vita.Credit.Dtos;
using vita.Debit.Dtos;
using vita.Sales.Dtos;
using vita.UblSharp.Dtos;
using vita.Utils;
using SignXML;
using SignXML.Models.Generate;
using SignXML.Models.Populate;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;
using SignXML.Models.Utils;
using Abp.EntityFrameworkCore;
using vita.EntityFrameworkCore;
using Abp.Runtime.Session;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using vita.Authorization;
using Abp.Authorization;
using NPOI.POIFS.Crypt.Dsig;
using System.Transactions;

namespace vita.UblSharp
{
    [AbpAuthorize(AppPermissions.Pages_SalesInvoices)]
    public class GenerateXmlAppService : vitaAppServiceBase,IGenerateXmlAppService
    {
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;

        public GenerateXmlAppService(IDbContextProvider<vitaDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public class XMLSignInput
        {
            public string xml { get; set; }

            public InvoiceType? xmlModel { get; set; }

            public string qrPath { get; set; }
            public string UUID { get; set; }

            public string SellerName { get; set; }
            public string VatId { get; set; }
            public decimal VatAmount { get; set; }

            public decimal TotalAmount { get; set; }

            public string xmlPath { get; set; }

            public string pathToSave { get; set; }
        }

        public class XMLLogInput{
            public string uuid { get; set; }
            public string signature { get; set; }
            public string certificate { get; set; }
            public string xml64 { get; set; }
            public string invoiceHash64 { get; set; }
            public string csid { get; set; }
            public string qrBase64 { get; set; }
            public string complianceInvoiceResponse { get; set; }
            public string reportInvoiceResponse { get; set; }
            public string clearanceResponse { get; set; }

        }

        public void LogXML(XMLLogInput input)
        {
           
            SqlConnection conn = null;
            try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (conn = new SqlConnection(connStr))
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {

                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "InsertXMLLog";
                        Guid guidOutput= Guid.NewGuid();
                        bool isValid = Guid.TryParse(input.uuid, out guidOutput);

                        cmd.Parameters.AddWithValue("@uuid", (isValid)?input.uuid:Guid.NewGuid().ToString());
                        cmd.Parameters.AddWithValue("@createdOn", DateTime.Now);
                        cmd.Parameters.AddWithValue("@createdBy", AbpSession.UserId ?? 0);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId ?? 0);
                        cmd.Parameters.AddWithValue("@signature", input.signature);
                        cmd.Parameters.AddWithValue("@certificate", input.certificate);
                        cmd.Parameters.AddWithValue("@xml64", input.xml64);
                        cmd.Parameters.AddWithValue("@invoiceHash64", input.invoiceHash64);
                        cmd.Parameters.AddWithValue("@csid", input.csid);
                        cmd.Parameters.AddWithValue("@qrBase64", input.qrBase64);
                        cmd.Parameters.AddWithValue("@complianceInvoiceResponse", input.complianceInvoiceResponse);
                        cmd.Parameters.AddWithValue("@reportInvoiceResponse", input.reportInvoiceResponse);
                        cmd.Parameters.AddWithValue("@clearanceResponse", input.clearanceResponse);

                        var reader = cmd.ExecuteNonQuery();

                    }

                }
            }
            catch (Exception e)
            {


            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }
        }


        public async Task<string> SignXml(XMLSignInput input)
        {
            try
            {


                Zatca.ConfigureOpenSSL(new OpenSSLConfig()
                {
                    useEnvironmentVariable = false,
                    useAssemblyEntryPoint = true,
                    pathToOpenSSL = "SignXML\\openssl\\binn"
                }); ;

                Zatca.ConfigureCertConf(new CertificateConfig()
                {
                    useEnvironmentDirectory = false,
                    useAssemblyEntryPoint = true,
                    pathToConfig="SignXML\\Configuration.cnf"
                });

                var xml = input.xml;
                xml = Zatca.GetXMLFromCustomMapping(input.xmlModel);
                var hash = Zatca.GetInvoiceHash(xml);
                var signature = Zatca.GetDigitalSignature(hash.InvoiceHash);


                var csr = Zatca.GetCSR(signature.EcdsaPrivateKey, null);

                var csid = await Zatca.GetCSIDAsync(csr.CertificateRequestBase64, "123345");

                //try at least thrice on failure
                var i = 0;
                while (csid.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    csid = await Zatca.GetCSIDAsync(csr.CertificateRequestBase64, "123345");
                    i++;
                    if (i > 3) return null;
                };



                var certificate = Zatca.GetCertificate(csid);

                xml = Zatca.AddSignedPropertiesToXML(xml, new ZatcaSignedProperties
                {
                    DigestValue = certificate.CertificateHash, //to be checked
                    SigningTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                    X509IssuerName = certificate.IssuerName,
                    X509SerialNumber = certificate.SerialNumber
                });

                var signedHash = Zatca.GenerateSignedPropertiesHash(xml);

                xml = Zatca.AddUBLExtensionsToXML(new ZatcaUblExtensions
                {
                    xml = xml,
                    SignatureValue = signature.SignatureBase64,
                    X509Certificate = certificate.CertificatePEM.Replace("-----BEGIN CERTIFICATE-----\n", "").Replace("\n-----END CERTIFICATE-----", ""),
                    EncodedSignedPropertiesHash = signedHash,
                    EncodedInvoiceHash = hash.InvoiceHashBase64
                });

                if (!Directory.Exists(input.pathToSave))
                    Directory.CreateDirectory(input.pathToSave);

                var qrBase64 = Zatca.GetQrCode(new ZatcaQrCode()
                {
                    SellerName = input.SellerName,
                    VatNumber = input.VatId,
                    IssueDateTime = DateTime.Now,
                    TotalAmount = input.TotalAmount,
                    VatAmount = input.VatAmount,
                    XmlHash = hash.InvoiceHashBase64,
                    Signature = signature.SignatureBase64,
                    PublicKey = certificate.PublicKey,
                    PublicKeySignature = certificate.Signature
                }, input.qrPath);

                xml = Zatca.AddQRToXML(xml, qrBase64);

               

                File.WriteAllText(input.xmlPath, xml);

                var complianceInvoice = await Zatca.ComplianceInvoiceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));

                var reportingRes = await Zatca.ReportInvoiceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));

                var clearing = await Zatca.ClearanceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));

                var log = new XMLLogInput()
                {
                    uuid = input.UUID,
                    signature = JsonConvert.SerializeObject(signature),
                    certificate = JsonConvert.SerializeObject(certificate),
                    xml64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)),
                    invoiceHash64 = hash.InvoiceHashBase64,
                    csid = JsonConvert.SerializeObject(csid),
                    qrBase64 = qrBase64,
                    complianceInvoiceResponse = JsonConvert.SerializeObject(complianceInvoice),
                    reportInvoiceResponse = JsonConvert.SerializeObject(reportingRes),
                    clearanceResponse = JsonConvert.SerializeObject(clearing)
                };

                    LogXML(log);
                

                return xml;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                LogXML(new XMLLogInput()
                {
                    uuid = input.UUID

                });
                return null;
            }
          
        }

 

        public async Task<bool> GenerateXmlRequest_Invoice(CreateOrEditSalesInvoiceDto input, string invoiceno, string uniqueIdentifier, string tenantId, string xml_uid = "")
        {

            var qrString = "";

            List<LineItem> items = new List<LineItem>();
            decimal taxTotal = 0; decimal price = 0; decimal total = 0; ; decimal discount = 0;


            int counter = 0; var type = "00";
            var countryCode = "SA"; string invoiceType = string.Empty;
            InvoiceRequestDto request = new InvoiceRequestDto();
            List<CreateOrEditSalesInvoiceItemDto> invItems = new List<CreateOrEditSalesInvoiceItemDto>();
            try
            {
                int invoiceLineCounter = input.Items.Count;

                foreach (var item in input.Items)
                {
                    counter = counter + 1;
                    var invoiceItem = new LineItem();

                    invoiceItem.Quantity = Convert.ToDecimal(item.Quantity);
                    invoiceItem.UnitPrice = Convert.ToDecimal(item.UnitPrice);
                    invoiceItem.DiscountPercentage = Convert.ToDecimal(item.DiscountPercentage);

                    decimal costPrice = invoiceItem.Quantity * invoiceItem.UnitPrice;
                    if (invoiceItem.DiscountPercentage != 0)
                        invoiceItem.DiscountAmount = costPrice * (invoiceItem.DiscountPercentage / 100);
                    else
                        invoiceItem.DiscountAmount = 0;

                    invoiceItem.VatRate = Convert.ToDecimal(item.VATRate);
                    invoiceItem.VatAmount = (costPrice - invoiceItem.DiscountAmount) * (invoiceItem.VatRate / 100);
                    if (taxTotal == 0)
                        taxTotal = invoiceItem.VatAmount;
                    else
                        taxTotal = taxTotal + invoiceItem.VatAmount;

                    invoiceItem.GrossPrice = costPrice - invoiceItem.DiscountAmount;
                    if (price == 0)
                        price = invoiceItem.GrossPrice;
                    else
                        price = price + invoiceItem.GrossPrice;

                    invoiceItem.NetPrice = invoiceItem.GrossPrice + invoiceItem.VatAmount;
                    if (total == 0)
                        total = invoiceItem.NetPrice;
                    else
                        total = total + invoiceItem.NetPrice;

                    discount += invoiceItem.DiscountAmount; invItems.Add(item);


                }

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                if (String.IsNullOrEmpty(input.VATDetails[0].VATCode) && total < 1000 && countryCode == "SA")
                {
                    type = "02";
                    invoiceType = "simplified";
                }
                else
                {
                    type = "01";
                    invoiceType = "standard";
                }
                builder.Append(type);
                builder.Append(request.ThirdParty ? "1" : "0");
                builder.Append(request.Nominal ? "1" : "0");
                builder.Append(request.Export ? "1" : "0");
                builder.Append(request.Summary ? "1" : "0");
                builder.Append(request.ThirdParty ? "1" : "0");
                request.InvoiceTransactionType = builder.ToString();

                XmlElement extensionContent = @"
                                    <sig:UBLDocumentSignatures   xmlns:cac= ""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"" xmlns:cbc= ""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"" xmlns:ext= ""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"" xmlns:sig=""urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2"" xmlns:sac=""urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2"" xmlns:sbc=""urn:oasis:names:specification:ubl:schema:xsd:SignatureBasicComponents-2"">
                <sac:SignatureInformation>
                    <cbc:ID>urn:oasis:names:specification:ubl:signature:1</cbc:ID>
                    <sbc:ReferencedSignatureID>urn:oasis:names:specification:ubl:signature:Invoice</sbc:ReferencedSignatureID>
                    <ds:Signature xmlns:ds=""http://www.w3.org/2000/09/xmldsig#"" Id=""signature"">
                        <ds:SignedInfo>
                            <ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2006/12/xml-c14n11""/>
                            <ds:SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256""/>
                            <ds:Reference Id=""invoiceSignedData"" URI="""">
                                <ds:Transforms>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::ext:UBLExtensions)</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::cac:Signature)</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::cac:AdditionalDocumentReference[cbc:ID='QR'])</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/2006/12/xml-c14n11""/>
                                </ds:Transforms>
                                <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                <ds:DigestValue>PEx8bNFcEMEpHzUVvQntQI6ot8eFqTT/l59b+H1HqX4=</ds:DigestValue>
                            </ds:Reference>
                            <ds:Reference Type=""http://www.w3.org/2000/09/xmldsig#SignatureProperties"" URI=""#xadesSignedProperties"">
                                <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                <ds:DigestValue>ZDEyMDUyODJjYzk4MGViNTJhNmYzMGIyZTgxODhkY2JlOWEzNmRiMTFlZTVhMDAxNjk5OTRkYTg3ODhlY2ZiMw==</ds:DigestValue>
                            </ds:Reference>
                        </ds:SignedInfo>
                        <ds:SignatureValue>MEUCIQC90fFYOqTimHvYP1f9bbT5stAfR8bI2fAAFAzYAvMCPQIgcGpGhMSocxfwdvcSW1B1523g5nD8bCe8SCWNect5rKM=</ds:SignatureValue>
                        <ds:KeyInfo>
                            <ds:X509Data>
                                <ds:X509Certificate>MIID6TCCA5CgAwIBAgITbwAAf8tem6jngr16DwABAAB/yzAKBggqhkjOPQQDAjBjMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxEzARBgoJkiaJk/IsZAEZFgNnb3YxFzAVBgoJkiaJk/IsZAEZFgdleHRnYXp0MRwwGgYDVQQDExNUU1pFSU5WT0lDRS1TdWJDQS0xMB4XDTIyMDkxNDEzMjYwNFoXDTI0MDkxMzEzMjYwNFowTjELMAkGA1UEBhMCU0ExEzARBgNVBAoTCjMxMTExMTExMTExDDAKBgNVBAsTA1RTVDEcMBoGA1UEAxMTVFNULTMxMTExMTExMTEwMTExMzBWMBAGByqGSM49AgEGBSuBBAAKA0IABGGDDKDmhWAITDv7LXqLX2cmr6+qddUkpcLCvWs5rC2O29W/hS4ajAK4Qdnahym6MaijX75Cg3j4aao7ouYXJ9GjggI5MIICNTCBmgYDVR0RBIGSMIGPpIGMMIGJMTswOQYDVQQEDDIxLVRTVHwyLVRTVHwzLWE4NjZiMTQyLWFjOWMtNDI0MS1iZjhlLTdmNzg3YTI2MmNlMjEfMB0GCgmSJomT8ixkAQEMDzMxMTExMTExMTEwMTExMzENMAsGA1UEDAwEMTEwMDEMMAoGA1UEGgwDVFNUMQwwCgYDVQQPDANUU1QwHQYDVR0OBBYEFDuWYlOzWpFN3no1WtyNktQdrA8JMB8GA1UdIwQYMBaAFHZgjPsGoKxnVzWdz5qspyuZNbUvME4GA1UdHwRHMEUwQ6BBoD+GPWh0dHA6Ly90c3RjcmwuemF0Y2EuZ292LnNhL0NlcnRFbnJvbGwvVFNaRUlOVk9JQ0UtU3ViQ0EtMS5jcmwwga0GCCsGAQUFBwEBBIGgMIGdMG4GCCsGAQUFBzABhmJodHRwOi8vdHN0Y3JsLnphdGNhLmdvdi5zYS9DZXJ0RW5yb2xsL1RTWkVpbnZvaWNlU0NBMS5leHRnYXp0Lmdvdi5sb2NhbF9UU1pFSU5WT0lDRS1TdWJDQS0xKDEpLmNydDArBggrBgEFBQcwAYYfaHR0cDovL3RzdGNybC56YXRjYS5nb3Yuc2Evb2NzcDAOBgNVHQ8BAf8EBAMCB4AwHQYDVR0lBBYwFAYIKwYBBQUHAwIGCCsGAQUFBwMDMCcGCSsGAQQBgjcVCgQaMBgwCgYIKwYBBQUHAwIwCgYIKwYBBQUHAwMwCgYIKoZIzj0EAwIDRwAwRAIgOgjNPJW017lsIijmVQVkP7GzFO2KQKd9GHaukLgIWFsCIFJF9uwKhTMxDjWbN+1awsnFI7RLBRxA/6hZ+F1wtaqU</ds:X509Certificate>
                            </ds:X509Data>
                        </ds:KeyInfo>
                        <ds:Object>
                            <xades:QualifyingProperties xmlns:xades=""http://uri.etsi.org/01903/v1.3.2#"" Target=""signature"">
                                <xades:SignedProperties Id=""xadesSignedProperties"">
                                    <xades:SignedSignatureProperties>
                                        <xades:SigningTime>2023-01-11T13:08:10Z</xades:SigningTime>
                                        <xades:SigningCertificate>
                                            <xades:Cert>
                                                <xades:CertDigest>
                                                    <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                                    <ds:DigestValue>YTJkM2JhYTcwZTBhZTAxOGYwODMyNzY3NTdkZDM3YzhjY2IxOTIyZDZhM2RlZGJiMGY0NDUzZWJhYWI4MDhmYg==</ds:DigestValue>
                                                </xades:CertDigest>
                                                <xades:IssuerSerial>
                                                    <ds:X509IssuerName>CN=TSZEINVOICE-SubCA-1, DC=extgazt, DC=gov, DC=local</ds:X509IssuerName>
                                                    <ds:X509SerialNumber>2475382886904809774818644480820936050208702411</ds:X509SerialNumber>
                                                </xades:IssuerSerial>
                                            </xades:Cert>
                                        </xades:SigningCertificate>
                                    </xades:SignedSignatureProperties>
                                </xades:SignedProperties>
                            </xades:QualifyingProperties>
                        </ds:Object>
                    </ds:Signature>
                </sac:SignatureInformation>
            </sig:UBLDocumentSignatures>
                               ".ToXmlElement();
                var doc = new InvoiceType();



                doc = new InvoiceType
                {
                    UBLExtensions = new List<UBLExtensionType>()
                      {
                      new UBLExtensionType
                      {

                        ExtensionURI="urn:oasis:names:specification:ubl:dsig:enveloped:xades",
                        ExtensionContent=extensionContent
                      }
                    },
                    ProfileID = "reporting:1.0",
                    ID = invoiceno.ToString(),
                    UUID = uniqueIdentifier.ToString(),
                    IssueDate = new DateType
                    {
                        Value = input.IssueDate //to be checked
                    },
                    IssueTime = new TimeType
                    {
                        Value = input.IssueDate //to be checked
                    },
                    InvoiceTypeCode = new CodeType
                    {
                        name = request.InvoiceTransactionType,
                        Value = "388"  //Invoice type specific
                    },
                    DocumentCurrencyCode = new CodeType
                    {
                        Value = "SAR"
                    },
                    TaxCurrencyCode = new CodeType
                    {
                        Value = "SAR"
                    },
                    LineCountNumeric = new NumericType
                    {
                        Value = invItems.Count
                    },
                    AdditionalDocumentReference = new List<DocumentReferenceType>()
                            {
                         new DocumentReferenceType
                                {
                                    ID = "ICV",
                                    UUID= invoiceno.ToString(),
                                },
                                new DocumentReferenceType
                                {
                                    ID = "PIH",
                                    Attachment = new AttachmentType
                                    {
                                        EmbeddedDocumentBinaryObject = new BinaryObjectType
                                        {
                                            mimeCode = "text/plain",
                                            Value = Convert.FromBase64String("NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==")
                                        }
                                    }
                                },

                                new DocumentReferenceType
                                {
                                    ID = "QR",
                                    Attachment = new AttachmentType
                                    {
                                        EmbeddedDocumentBinaryObject = new BinaryObjectType
                                        {
                                            mimeCode = "text/plain",
                                            Value = Convert.FromBase64String(qrString)
                                        }
                                    }
                                }
                            },
                    Signature = new List<SignatureType>()
                            {
                              new SignatureType
                              {
                                ID="urn:oasis:names:specification:ubl:signature:Invoice",
                                SignatureMethod="urn:oasis:names:specification:ubl:dsig:enveloped:xades"
                              }
                            },
                    AccountingSupplierParty = new SupplierPartyType
                    {
                        Party = new PartyType
                        {
                            PartyIdentification = new List<PartyIdentificationType>()
                                {
                                    new PartyIdentificationType
                                    {
                                        ID = new IdentifierType
                                        {
                                            schemeID = "CRN",
                                            Value = input.Supplier.CRNumber
                                        }
                                    }
                                },

                            PostalAddress = new AddressType
                            {
                                StreetName = input.Supplier.Address.Street ?? "",
                                BuildingNumber = input.Supplier.Address.BuildingNo ?? "",
                                PlotIdentification = input.Supplier.Address.AdditionalNo ?? "",
                                CitySubdivisionName = input.Supplier.Address.AdditionalStreet ?? "",
                                CityName = input.Supplier.Address.City ?? "",
                                PostalZone = input.Supplier.Address.PostalCode ?? "",
                                CountrySubentity = input.Supplier.Address.State ?? "",
                                Country = new CountryType
                                {
                                    IdentificationCode = new CodeType
                                    {
                                        Value = input.Supplier.Address.CountryCode ?? ""
                                    }
                                }
                            },
                            PartyTaxScheme = new List<PartyTaxSchemeType>()
                                    {
                                        new PartyTaxSchemeType
                                        {
                                            CompanyID = new IdentifierType
                                            {
                                                  Value = input.Supplier.VATID
                                            },
                                            TaxScheme = new TaxSchemeType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "VAT"
                                                }
                                            }
                                        }
                                    },
                            PartyLegalEntity = new List<PartyLegalEntityType>()
                                    {
                                        new PartyLegalEntityType
                                        {
                                            RegistrationName = input.Supplier.RegistrationName
                                        }
                                    }
                        }
                    },
                    AccountingCustomerParty = new CustomerPartyType
                    {
                        Party = new PartyType
                        {
                            PartyIdentification = new List<PartyIdentificationType>()
                                {
                                    new PartyIdentificationType
                                    {
                                        ID = new IdentifierType
                                        {
                                            schemeID = "NAT",
                                            Value = "2345" //need information
                                        }
                                    }
                                },
                            //Person = new List<PersonType>()
                            //    {
                            //        new PersonType{
                            //        Contact = new ContactType
                            //        {
                            //            ElectronicMail = input.Buyer.ContactPerson.Email,
                            //            Telephone = input.Buyer.ContactPerson.ContactNumber,
                            //            Name = input.Buyer.ContactPerson.Name

                            //        }       //recheck
                            //        }
                            //    },

                            PostalAddress = new AddressType
                            {
                                StreetName = input.Buyer.Address.Street ?? "",
                                BuildingNumber = input.Buyer.Address.BuildingNo ?? "",
                                PlotIdentification = input.Buyer.Address.AdditionalNo ?? "",
                                CitySubdivisionName = input.Buyer.Address.AdditionalStreet ?? "",
                                CityName = input.Buyer.Address.City ?? "",
                                PostalZone = input.Buyer.Address.PostalCode ?? "",
                                CountrySubentity = input.Buyer.Address.State ?? "",
                                Country = new CountryType
                                {
                                    IdentificationCode = new CodeType
                                    {
                                        Value = input.Supplier.Address.CountryCode
                                    }
                                }
                            },
                            PartyTaxScheme = new List<PartyTaxSchemeType>()
                                    {
                                        new PartyTaxSchemeType
                                        {
                                            TaxScheme = new TaxSchemeType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "VAT"
                                                }
                                            },
                                            CompanyID = new IdentifierType
                                            {
                                                Value=input.Buyer.VATID
                                            }
                                        }
                                    },
                            PartyLegalEntity = new List<PartyLegalEntityType>()
                                    {
                                        new PartyLegalEntityType
                                        {
                                           RegistrationName = input.Supplier.RegistrationName
                                        }
                                    }
                        }
                    },
                    //Delivery = new List<DeliveryType>()
                    //{
                    //    new DeliveryType
                    //    {
                    //        ActualDeliveryDate = input.LatestDeliveryDate
                    //    }
                    //},
                    PaymentMeans = new List<PaymentMeansType>()
                              {
                                new PaymentMeansType
                                {
                                    PaymentMeansCode = new CodeType
                                    {
                                        Value = "42" //need information
                                    }
                                }
                              },
                    AllowanceCharge = new List<AllowanceChargeType>()
                    {
                        new AllowanceChargeType
                        {
                            ID = new IdentifierType
                            {
                                Value="1"
                            },
                            ChargeIndicator = new IndicatorType
                            {
                                Value=false
                            },
                            AllowanceChargeReason = new List<TextType>()
                            {
                                new TextType()
                                {
                                    Value = "discount"
                                }
                            },
                            Amount = new AmountType
                            {
                                currencyID="SAR",
                                Value= Math.Round(discount,2)
                            },
                            TaxCategory = new List<TaxCategoryType>()
                            {
                                new TaxCategoryType
                                        {
                                                        ID = new IdentifierType
                                                        {
                                                            schemeAgencyID="6",
                                                            schemeID="UN/ECE 5305",  //need information
                                                            Value = "S"
                                                        },
                                                        Percent = 15M,
                                                        TaxScheme = new TaxSchemeType
                                                        {
                                                            ID = new IdentifierType
                                                            {
                                                                schemeAgencyID="6",
                                                                schemeID="UN/ECE 5153",  //need information
                                                                Value = "VAT",
                                                            }
                                                        }
                                                    }
                            }
                        }
                    },
                    TaxTotal = new List<TaxTotalType>()
                                   {
                                        new TaxTotalType
                                        {
                                            TaxAmount = new AmountType
                                            {
                                                currencyID = "SAR",
                                                Value =Math.Round(taxTotal,2) //135.00M
                                            },

                                            TaxSubtotal = new List<TaxSubtotalType>()
                                            {
                                                new TaxSubtotalType
                                                {
                                                    TaxableAmount = new AmountType
                                                    {
                                                        currencyID = "SAR",
                                                        Value =Math.Round(price,2)  //90Math.Round(discount,2) 
                                                    },
                                                    TaxAmount = new AmountType
                                                    {
                                                        currencyID = "SAR",
                                                        Value = Math.Round(taxTotal,2) //135.00M 
                                                    },
                                                    TaxCategory = new TaxCategoryType
                                                    {
                                                        ID = new IdentifierType
                                                        {
                                                            schemeAgencyID="6",
                                                            schemeID="UN/ECE 5305",  //need information
                                                            Value = "S"
                                                        },
                                                        Percent = 15M,
                                                        TaxScheme = new TaxSchemeType
                                                        {
                                                            ID = new IdentifierType
                                                            {
                                                                schemeAgencyID="6",
                                                                schemeID="UN/ECE 5153",  //need information
                                                                Value = "VAT",
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new TaxTotalType
                                        {
                                            TaxAmount=new AmountType
                                            {
                                                currencyID="SAR",
                                                Value=Math.Round(taxTotal,2)
                                            }
                                        }
                                   },
                    LegalMonetaryTotal = new MonetaryTotalType
                    {
                        LineExtensionAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(price + discount, 2)
                        },
                        TaxExclusiveAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(price, 2)
                        },
                        TaxInclusiveAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(total, 2)
                        },
                        AllowanceTotalAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(discount, 2)
                        },
                        PayableAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(total, 2)
                        }
                    },
                    InvoiceLine = new List<InvoiceLineType>()

                };

                var x = 0;
                foreach (var item in invItems)
                {
                    x++;
                    doc.InvoiceLine.Add(

                        new InvoiceLineType
                        {
                            ID = x.ToString(),
                            InvoicedQuantity = new QuantityType
                            {
                                unitCode = "PCE", //need information
                                Value = item.Quantity
                            },
                            LineExtensionAmount = new AmountType
                            {
                                currencyID = "SAR",
                                Value = Math.Round(item.Quantity * item.UnitPrice - item.DiscountAmount, 2)
                            },
                            TaxTotal = new List<TaxTotalType>()
                                {
                                        new TaxTotalType
                                        {
                                            TaxAmount = new AmountType
                                            {
                                                currencyID = "SAR",
                                                Value = Math.Round(item.VATAmount,2)
                                            },
                                            RoundingAmount = new AmountType
                                            {
                                              currencyID= "SAR",
                                              Value=Math.Round(item.LineAmountInclusiveVAT,2)
                                            }
                                        }
                                },
                            Item = new ItemType

                            {
                                Name = item.Name + " " + item.Description,

                                ClassifiedTaxCategory = new List<TaxCategoryType>()
                                    {
                                            new TaxCategoryType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "S"
                                                },
                                                Percent = 15M,
                                                TaxScheme = new TaxSchemeType
                                                {
                                                    ID = new IdentifierType
                                                    {
                                                        Value = "VAT"
                                                    }
                                                }
                                            }
                                    }
                            },
                            Price = new PriceType
                            {
                                PriceAmount = new AmountType
                                {
                                    currencyID = "SAR",
                                    Value = Math.Round(item.UnitPrice, 2)
                                },
                                //BaseQuantity = new QuantityType
                                //{
                                //    unitCode = "PCE",
                                //    Value = item.Quantity
                                //},
                                AllowanceCharge = new List<AllowanceChargeType>
                                    {
                                            new AllowanceChargeType {
                                                 ChargeIndicator=false,
                                           //      MultiplierFactorNumeric = item.DiscountPercentage,
                                        Amount=new AmountType
                                        {
                                            currencyID="SAR",
                                            Value = Math.Round(item.DiscountAmount,2)
                                        },
                                        AllowanceChargeReason=new List<TextType>()
                                        {
                                            new TextType
                                            {
                                                Value="discount"
                                            }
                                        }
                                        //BaseAmount = new AmountType
                                        //{
                                        //    currencyID="SAR",
                                        //    Value = Math.Round(item.UnitPrice,2)
                                        //}
                                       }
                                  }
                            }


                        });
                }

                doc.Xmlns = new System.Xml.Serialization.XmlSerializerNamespaces(new[] {
                            new XmlQualifiedName(null,"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"),
                            new XmlQualifiedName("cac","urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"),
                            new XmlQualifiedName("cbc","urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"),
                            new XmlQualifiedName("ext","urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")
                        });


                var pathToSave = string.Empty;
                if (tenantId != null && tenantId != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, uniqueIdentifier);
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0", uniqueIdentifier);
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
                var path = (Path.Combine(pathToSave, xmlfileName));

          
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(InvoiceType));
                var xml = "";

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, doc, doc.Xmlns);
                    xml= textWriter.ToString();
                }

                await SignXml(new XMLSignInput()
                {
                    xml = xml,
                    UUID= xml_uid,
                    qrPath = Path.Combine(pathToSave,uniqueIdentifier+"_"+invoiceno+".png"),
                    SellerName=input.Supplier.RegistrationName,
                    VatId = input.Supplier.VATID,
                    TotalAmount = input.InvoiceSummary.TotalAmountWithVAT,
                    VatAmount = input.InvoiceSummary.TotalVATAmount,
                    xmlPath = path,
                    pathToSave = pathToSave,
                    xmlModel = doc
                });

                return true;

            }

            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> GenerateXmlRequest_CreditNote(CreateOrEditCreditNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId, string xml_uid = "")
        {
            var qrString = "";


            List<LineItem> items = new List<LineItem>();
            decimal taxTotal = 0; decimal price = 0; decimal total = 0; ; decimal discount = 0;


            int counter = 0; var type = "00";
            var countryCode = "SA"; string invoiceType = string.Empty;
            InvoiceRequestDto request = new InvoiceRequestDto();
            List<CreateOrEditCreditNoteItemDto> invItems = new List<CreateOrEditCreditNoteItemDto>();
            try
            {
                int invoiceLineCounter = input.Items.Count;

                foreach (var item in input.Items)
                {
                    counter = counter + 1;
                    var invoiceItem = new LineItem();

                    invoiceItem.Quantity = Convert.ToDecimal(item.Quantity);
                    invoiceItem.UnitPrice = Convert.ToDecimal(item.UnitPrice);
                    invoiceItem.DiscountPercentage = Convert.ToDecimal(item.DiscountPercentage);

                    decimal costPrice = invoiceItem.Quantity * invoiceItem.UnitPrice;
                    if (invoiceItem.DiscountPercentage != 0)
                        invoiceItem.DiscountAmount = costPrice * (invoiceItem.DiscountPercentage / 100);
                    else
                        invoiceItem.DiscountAmount = 0;

                    invoiceItem.VatRate = Convert.ToDecimal(item.VATRate);
                    invoiceItem.VatAmount = (costPrice - invoiceItem.DiscountAmount) * (invoiceItem.VatRate / 100);
                    if (taxTotal == 0)
                        taxTotal = invoiceItem.VatAmount;
                    else
                        taxTotal = taxTotal + invoiceItem.VatAmount;

                    invoiceItem.GrossPrice = costPrice - invoiceItem.DiscountAmount;
                    if (price == 0)
                        price = invoiceItem.GrossPrice;
                    else
                        price = price + invoiceItem.GrossPrice;

                    invoiceItem.NetPrice = invoiceItem.GrossPrice + invoiceItem.VatAmount;
                    if (total == 0)
                        total = invoiceItem.NetPrice;
                    else
                        total = total + invoiceItem.NetPrice;

                    discount += invoiceItem.DiscountAmount; invItems.Add(item);


                }

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                if (String.IsNullOrEmpty(input.VATDetails[0].VATCode) && total < 1000 && countryCode == "SA")
                {
                    type = "02";
                    invoiceType = "simplified";
                }
                else
                {
                    type = "01";
                    invoiceType = "standard";
                }
                builder.Append(type);
                builder.Append(request.ThirdParty ? "1" : "0");
                builder.Append(request.Nominal ? "1" : "0");
                builder.Append(request.Export ? "1" : "0");
                builder.Append(request.Summary ? "1" : "0");
                builder.Append(request.ThirdParty ? "1" : "0");
                request.InvoiceTransactionType = builder.ToString();

                XmlElement extensionContent = @"
                                    <sig:UBLDocumentSignatures   xmlns:cac= ""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"" xmlns:cbc= ""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"" xmlns:ext= ""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"" xmlns:sig=""urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2"" xmlns:sac=""urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2"" xmlns:sbc=""urn:oasis:names:specification:ubl:schema:xsd:SignatureBasicComponents-2"">
                <sac:SignatureInformation>
                    <cbc:ID>urn:oasis:names:specification:ubl:signature:1</cbc:ID>
                    <sbc:ReferencedSignatureID>urn:oasis:names:specification:ubl:signature:Invoice</sbc:ReferencedSignatureID>
                    <ds:Signature xmlns:ds=""http://www.w3.org/2000/09/xmldsig#"" Id=""signature"">
                        <ds:SignedInfo>
                            <ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2006/12/xml-c14n11""/>
                            <ds:SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256""/>
                            <ds:Reference Id=""invoiceSignedData"" URI="""">
                                <ds:Transforms>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::ext:UBLExtensions)</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::cac:Signature)</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::cac:AdditionalDocumentReference[cbc:ID='QR'])</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/2006/12/xml-c14n11""/>
                                </ds:Transforms>
                                <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                <ds:DigestValue>PEx8bNFcEMEpHzUVvQntQI6ot8eFqTT/l59b+H1HqX4=</ds:DigestValue>
                            </ds:Reference>
                            <ds:Reference Type=""http://www.w3.org/2000/09/xmldsig#SignatureProperties"" URI=""#xadesSignedProperties"">
                                <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                <ds:DigestValue>ZDEyMDUyODJjYzk4MGViNTJhNmYzMGIyZTgxODhkY2JlOWEzNmRiMTFlZTVhMDAxNjk5OTRkYTg3ODhlY2ZiMw==</ds:DigestValue>
                            </ds:Reference>
                        </ds:SignedInfo>
                        <ds:SignatureValue>MEUCIQC90fFYOqTimHvYP1f9bbT5stAfR8bI2fAAFAzYAvMCPQIgcGpGhMSocxfwdvcSW1B1523g5nD8bCe8SCWNect5rKM=</ds:SignatureValue>
                        <ds:KeyInfo>
                            <ds:X509Data>
                                <ds:X509Certificate>MIID6TCCA5CgAwIBAgITbwAAf8tem6jngr16DwABAAB/yzAKBggqhkjOPQQDAjBjMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxEzARBgoJkiaJk/IsZAEZFgNnb3YxFzAVBgoJkiaJk/IsZAEZFgdleHRnYXp0MRwwGgYDVQQDExNUU1pFSU5WT0lDRS1TdWJDQS0xMB4XDTIyMDkxNDEzMjYwNFoXDTI0MDkxMzEzMjYwNFowTjELMAkGA1UEBhMCU0ExEzARBgNVBAoTCjMxMTExMTExMTExDDAKBgNVBAsTA1RTVDEcMBoGA1UEAxMTVFNULTMxMTExMTExMTEwMTExMzBWMBAGByqGSM49AgEGBSuBBAAKA0IABGGDDKDmhWAITDv7LXqLX2cmr6+qddUkpcLCvWs5rC2O29W/hS4ajAK4Qdnahym6MaijX75Cg3j4aao7ouYXJ9GjggI5MIICNTCBmgYDVR0RBIGSMIGPpIGMMIGJMTswOQYDVQQEDDIxLVRTVHwyLVRTVHwzLWE4NjZiMTQyLWFjOWMtNDI0MS1iZjhlLTdmNzg3YTI2MmNlMjEfMB0GCgmSJomT8ixkAQEMDzMxMTExMTExMTEwMTExMzENMAsGA1UEDAwEMTEwMDEMMAoGA1UEGgwDVFNUMQwwCgYDVQQPDANUU1QwHQYDVR0OBBYEFDuWYlOzWpFN3no1WtyNktQdrA8JMB8GA1UdIwQYMBaAFHZgjPsGoKxnVzWdz5qspyuZNbUvME4GA1UdHwRHMEUwQ6BBoD+GPWh0dHA6Ly90c3RjcmwuemF0Y2EuZ292LnNhL0NlcnRFbnJvbGwvVFNaRUlOVk9JQ0UtU3ViQ0EtMS5jcmwwga0GCCsGAQUFBwEBBIGgMIGdMG4GCCsGAQUFBzABhmJodHRwOi8vdHN0Y3JsLnphdGNhLmdvdi5zYS9DZXJ0RW5yb2xsL1RTWkVpbnZvaWNlU0NBMS5leHRnYXp0Lmdvdi5sb2NhbF9UU1pFSU5WT0lDRS1TdWJDQS0xKDEpLmNydDArBggrBgEFBQcwAYYfaHR0cDovL3RzdGNybC56YXRjYS5nb3Yuc2Evb2NzcDAOBgNVHQ8BAf8EBAMCB4AwHQYDVR0lBBYwFAYIKwYBBQUHAwIGCCsGAQUFBwMDMCcGCSsGAQQBgjcVCgQaMBgwCgYIKwYBBQUHAwIwCgYIKwYBBQUHAwMwCgYIKoZIzj0EAwIDRwAwRAIgOgjNPJW017lsIijmVQVkP7GzFO2KQKd9GHaukLgIWFsCIFJF9uwKhTMxDjWbN+1awsnFI7RLBRxA/6hZ+F1wtaqU</ds:X509Certificate>
                            </ds:X509Data>
                        </ds:KeyInfo>
                        <ds:Object>
                            <xades:QualifyingProperties xmlns:xades=""http://uri.etsi.org/01903/v1.3.2#"" Target=""signature"">
                                <xades:SignedProperties Id=""xadesSignedProperties"">
                                    <xades:SignedSignatureProperties>
                                        <xades:SigningTime>2023-01-11T13:08:10Z</xades:SigningTime>
                                        <xades:SigningCertificate>
                                            <xades:Cert>
                                                <xades:CertDigest>
                                                    <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                                    <ds:DigestValue>YTJkM2JhYTcwZTBhZTAxOGYwODMyNzY3NTdkZDM3YzhjY2IxOTIyZDZhM2RlZGJiMGY0NDUzZWJhYWI4MDhmYg==</ds:DigestValue>
                                                </xades:CertDigest>
                                                <xades:IssuerSerial>
                                                    <ds:X509IssuerName>CN=TSZEINVOICE-SubCA-1, DC=extgazt, DC=gov, DC=local</ds:X509IssuerName>
                                                    <ds:X509SerialNumber>2475382886904809774818644480820936050208702411</ds:X509SerialNumber>
                                                </xades:IssuerSerial>
                                            </xades:Cert>
                                        </xades:SigningCertificate>
                                    </xades:SignedSignatureProperties>
                                </xades:SignedProperties>
                            </xades:QualifyingProperties>
                        </ds:Object>
                    </ds:Signature>
                </sac:SignatureInformation>
            </sig:UBLDocumentSignatures>
                               ".ToXmlElement();
                var doc = new InvoiceType();



                doc = new InvoiceType
                {
                    UBLExtensions = new List<UBLExtensionType>()
                      {
                      new UBLExtensionType
                      {

                        ExtensionURI="urn:oasis:names:specification:ubl:dsig:enveloped:xades",
                        ExtensionContent=extensionContent
                      }
                    },
                    ProfileID = "reporting:1.0",
                    ID = invoiceno.ToString(),
                    UUID = uniqueIdentifier.ToString(),
                    IssueDate = new DateType
                    {
                        Value = input.IssueDate //to be checked
                    },
                    IssueTime = new TimeType
                    {
                        Value = input.IssueDate //to be checked
                    },
                    InvoiceTypeCode = new CodeType
                    {
                        name = request.InvoiceTransactionType,
                        Value = "388"  //Invoice type specific
                    },
                    DocumentCurrencyCode = new CodeType
                    {
                        Value = "SAR"
                    },
                    TaxCurrencyCode = new CodeType
                    {
                        Value = "SAR"
                    },
                    LineCountNumeric = new NumericType
                    {
                        Value = invItems.Count
                    },
                    AdditionalDocumentReference = new List<DocumentReferenceType>()
                            {
                         new DocumentReferenceType
                                {
                                    ID = "ICV",
                                    UUID= invoiceno.ToString(),
                                },
                                new DocumentReferenceType
                                {
                                    ID = "PIH",
                                    Attachment = new AttachmentType
                                    {
                                        EmbeddedDocumentBinaryObject = new BinaryObjectType
                                        {
                                            mimeCode = "text/plain",
                                            Value = Convert.FromBase64String("NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==")
                                        }
                                    }
                                },

                                new DocumentReferenceType
                                {
                                    ID = "QR",
                                    Attachment = new AttachmentType
                                    {
                                        EmbeddedDocumentBinaryObject = new BinaryObjectType
                                        {
                                            mimeCode = "text/plain",
                                            Value = Convert.FromBase64String(qrString)
                                        }
                                    }
                                }
                            },
                    Signature = new List<SignatureType>()
                            {
                              new SignatureType
                              {
                                ID="urn:oasis:names:specification:ubl:signature:Invoice",
                                SignatureMethod="urn:oasis:names:specification:ubl:dsig:enveloped:xades"
                              }
                            },
                    AccountingSupplierParty = new SupplierPartyType
                    {
                        Party = new PartyType
                        {
                            PartyIdentification = new List<PartyIdentificationType>()
                                {
                                    new PartyIdentificationType
                                    {
                                        ID = new IdentifierType
                                        {
                                            schemeID = "CRN",
                                            Value = input.Supplier.CRNumber
                                        }
                                    }
                                },

                            PostalAddress = new AddressType
                            {
                                StreetName = input.Supplier.Address.Street ?? "",
                                BuildingNumber = input.Supplier.Address.BuildingNo ?? "",
                                PlotIdentification = input.Supplier.Address.AdditionalNo ?? "",
                                CitySubdivisionName = input.Supplier.Address.AdditionalStreet ?? "",
                                CityName = input.Supplier.Address.City ?? "",
                                PostalZone = input.Supplier.Address.PostalCode ?? "",
                                CountrySubentity = input.Supplier.Address.State ?? "",
                                Country = new CountryType
                                {
                                    IdentificationCode = new CodeType
                                    {
                                        Value = input.Supplier.Address.CountryCode ?? ""
                                    }
                                }
                            },
                            PartyTaxScheme = new List<PartyTaxSchemeType>()
                                    {
                                        new PartyTaxSchemeType
                                        {
                                            CompanyID = new IdentifierType
                                            {
                                                  Value = input.Supplier.VATID
                                            },
                                            TaxScheme = new TaxSchemeType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "VAT"
                                                }
                                            }
                                        }
                                    },
                            PartyLegalEntity = new List<PartyLegalEntityType>()
                                    {
                                        new PartyLegalEntityType
                                        {
                                            RegistrationName = input.Supplier.RegistrationName
                                        }
                                    }
                        }
                    },
                    AccountingCustomerParty = new CustomerPartyType
                    {
                        Party = new PartyType
                        {
                            PartyIdentification = new List<PartyIdentificationType>()
                                {
                                    new PartyIdentificationType
                                    {
                                        ID = new IdentifierType
                                        {
                                            schemeID = "NAT",
                                            Value = "2345" //need information
                                        }
                                    }
                                },
                            //Person = new List<PersonType>()
                            //    {
                            //        new PersonType{
                            //        Contact = new ContactType
                            //        {
                            //            ElectronicMail = input.Buyer.ContactPerson.Email,
                            //            Telephone = input.Buyer.ContactPerson.ContactNumber,
                            //            Name = input.Buyer.ContactPerson.Name

                            //        }       //recheck
                            //        }
                            //    },

                            PostalAddress = new AddressType
                            {
                                StreetName = input.Buyer.Address.Street ?? "",
                                BuildingNumber = input.Buyer.Address.BuildingNo ?? "",
                                PlotIdentification = input.Buyer.Address.AdditionalNo ?? "",
                                CitySubdivisionName = input.Buyer.Address.AdditionalStreet ?? "",
                                CityName = input.Buyer.Address.City ?? "",
                                PostalZone = input.Buyer.Address.PostalCode ?? "",
                                CountrySubentity = input.Buyer.Address.State ?? "",
                                Country = new CountryType
                                {
                                    IdentificationCode = new CodeType
                                    {
                                        Value = input.Supplier.Address.CountryCode
                                    }
                                }
                            },
                            PartyTaxScheme = new List<PartyTaxSchemeType>()
                                    {
                                        new PartyTaxSchemeType
                                        {
                                            TaxScheme = new TaxSchemeType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "VAT"
                                                }
                                            },
                                            CompanyID = new IdentifierType
                                            {
                                                Value=input.Buyer.VATID
                                            }
                                        }
                                    },
                            PartyLegalEntity = new List<PartyLegalEntityType>()
                                    {
                                        new PartyLegalEntityType
                                        {
                                           RegistrationName = input.Supplier.RegistrationName
                                        }
                                    }
                        }
                    },
                    //Delivery = new List<DeliveryType>()
                    //{
                    //    new DeliveryType
                    //    {
                    //        ActualDeliveryDate = input.LatestDeliveryDate
                    //    }
                    //},
                    PaymentMeans = new List<PaymentMeansType>()
                              {
                                new PaymentMeansType
                                {
                                    PaymentMeansCode = new CodeType
                                    {
                                        Value = "42" //need information
                                    }
                                }
                              },
                    AllowanceCharge = new List<AllowanceChargeType>()
                    {
                        new AllowanceChargeType
                        {
                            ID = new IdentifierType
                            {
                                Value="1"
                            },
                            ChargeIndicator = new IndicatorType
                            {
                                Value=false
                            },
                            AllowanceChargeReason = new List<TextType>()
                            {
                                new TextType()
                                {
                                    Value = "discount"
                                }
                            },
                            Amount = new AmountType
                            {
                                currencyID="SAR",
                                Value= Math.Round(discount,2)
                            },
                            TaxCategory = new List<TaxCategoryType>()
                            {
                                new TaxCategoryType
                                        {
                                                        ID = new IdentifierType
                                                        {
                                                            schemeAgencyID="6",
                                                            schemeID="UN/ECE 5305",  //need information
                                                            Value = "S"
                                                        },
                                                        Percent = 15M,
                                                        TaxScheme = new TaxSchemeType
                                                        {
                                                            ID = new IdentifierType
                                                            {
                                                                schemeAgencyID="6",
                                                                schemeID="UN/ECE 5153",  //need information
                                                                Value = "VAT",
                                                            }
                                                        }
                                                    }
                            }
                        }
                    },
                    TaxTotal = new List<TaxTotalType>()
                                   {
                                        new TaxTotalType
                                        {
                                            TaxAmount = new AmountType
                                            {
                                                currencyID = "SAR",
                                                Value =Math.Round(taxTotal,2) //135.00M
                                            },

                                            TaxSubtotal = new List<TaxSubtotalType>()
                                            {
                                                new TaxSubtotalType
                                                {
                                                    TaxableAmount = new AmountType
                                                    {
                                                        currencyID = "SAR",
                                                        Value =Math.Round(price,2)  //90Math.Round(discount,2) 
                                                    },
                                                    TaxAmount = new AmountType
                                                    {
                                                        currencyID = "SAR",
                                                        Value = Math.Round(taxTotal,2) //135.00M 
                                                    },
                                                    TaxCategory = new TaxCategoryType
                                                    {
                                                        ID = new IdentifierType
                                                        {
                                                            schemeAgencyID="6",
                                                            schemeID="UN/ECE 5305",  //need information
                                                            Value = "S"
                                                        },
                                                        Percent = 15M,
                                                        TaxScheme = new TaxSchemeType
                                                        {
                                                            ID = new IdentifierType
                                                            {
                                                                schemeAgencyID="6",
                                                                schemeID="UN/ECE 5153",  //need information
                                                                Value = "VAT",
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new TaxTotalType
                                        {
                                            TaxAmount=new AmountType
                                            {
                                                currencyID="SAR",
                                                Value=Math.Round(taxTotal,2)
                                            }
                                        }
                                   },
                    LegalMonetaryTotal = new MonetaryTotalType
                    {
                        LineExtensionAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(price + discount, 2)
                        },
                        TaxExclusiveAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(price, 2)
                        },
                        TaxInclusiveAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(total, 2)
                        },
                        AllowanceTotalAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(discount, 2)
                        },
                        PayableAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(total, 2)
                        }
                    },
                    InvoiceLine = new List<InvoiceLineType>()

                };

                var x = 0;
                foreach (var item in invItems)
                {
                    x++;
                    doc.InvoiceLine.Add(

                        new InvoiceLineType
                        {
                            ID = x.ToString(),
                            InvoicedQuantity = new QuantityType
                            {
                                unitCode = "PCE", //need information
                                Value = item.Quantity
                            },
                            LineExtensionAmount = new AmountType
                            {
                                currencyID = "SAR",
                                Value = Math.Round(item.Quantity * item.UnitPrice - Convert.ToDecimal(item.DiscountAmount), 2)
                            },
                            TaxTotal = new List<TaxTotalType>()
                                {
                                        new TaxTotalType
                                        {
                                            TaxAmount = new AmountType
                                            {
                                                currencyID = "SAR",
                                                Value = Math.Round(item.VATAmount,2)
                                            },
                                            RoundingAmount = new AmountType
                                            {
                                              currencyID= "SAR",
                                              Value=Math.Round(item.LineAmountInclusiveVAT,2)
                                            }
                                        }
                                },
                            Item = new ItemType

                            {
                                Name = item.Name + " " + item.Description,

                                ClassifiedTaxCategory = new List<TaxCategoryType>()
                                    {
                                            new TaxCategoryType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "S"
                                                },
                                                Percent = 15M,
                                                TaxScheme = new TaxSchemeType
                                                {
                                                    ID = new IdentifierType
                                                    {
                                                        Value = "VAT"
                                                    }
                                                }
                                            }
                                    }
                            },
                            Price = new PriceType
                            {
                                PriceAmount = new AmountType
                                {
                                    currencyID = "SAR",
                                    Value = Math.Round(item.UnitPrice, 2)
                                },
                                //BaseQuantity = new QuantityType
                                //{
                                //    unitCode = "PCE",
                                //    Value = item.Quantity
                                //},
                                AllowanceCharge = new List<AllowanceChargeType>
                                    {
                                            new AllowanceChargeType {
                                                 ChargeIndicator=false,
                                           //      MultiplierFactorNumeric = item.DiscountPercentage,
                                        Amount=new AmountType
                                        {
                                            currencyID="SAR",
                                            Value = Math.Round( Convert.ToDecimal(item.DiscountAmount),2)
                                        },
                                        AllowanceChargeReason=new List<TextType>()
                                        {
                                            new TextType
                                            {
                                                Value="discount"
                                            }
                                        }
                                        //BaseAmount = new AmountType
                                        //{
                                        //    currencyID="SAR",
                                        //    Value = Math.Round(item.UnitPrice,2)
                                        //}
                                       }
                                  }
                            }


                        });
                }

                doc.Xmlns = new System.Xml.Serialization.XmlSerializerNamespaces(new[] {
                            new XmlQualifiedName(null,"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"),
                            new XmlQualifiedName("cac","urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"),
                            new XmlQualifiedName("cbc","urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"),
                            new XmlQualifiedName("ext","urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")
                        });


                var pathToSave = string.Empty;
                if (tenantId != null && tenantId != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, uniqueIdentifier);
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0", uniqueIdentifier);
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
                var path = (Path.Combine(pathToSave, xmlfileName));

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(InvoiceType));
                var xml = "";

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, doc, doc.Xmlns);
                    xml = textWriter.ToString();
                }

                await SignXml(new XMLSignInput()
                {
                    xml = xml,
                    UUID = input.Additional_Info,
                    qrPath = Path.Combine(pathToSave, uniqueIdentifier + "_" + invoiceno + ".png"),
                    SellerName = input.Supplier.RegistrationName,
                    VatId = input.Supplier.VATID,
                    TotalAmount = input.InvoiceSummary.TotalAmountWithVAT,
                    VatAmount = input.InvoiceSummary.TotalVATAmount,
                    xmlPath = path,
                    pathToSave = pathToSave,
                    xmlModel = doc
                });

                return true;

            }

            catch (Exception ex)
            {
                return false;
            }

      

        }

        public async Task<bool> GenerateXmlRequest_DebitNote(CreateOrEditDebitNoteDto input, string invoiceno, string uniqueIdentifier, string tenantId, string xml_uid = "")
        {
            var qrString = "";


            List<LineItem> items = new List<LineItem>();
            decimal taxTotal = 0; decimal price = 0; decimal total = 0; ; decimal discount = 0;


            int counter = 0; var type = "00";
            var countryCode = "SA"; string invoiceType = string.Empty;
            InvoiceRequestDto request = new InvoiceRequestDto();
            List<CreateOrEditDebitNoteItemDto> invItems = new List<CreateOrEditDebitNoteItemDto>();
            try
            {
                int invoiceLineCounter = input.Items.Count;

                foreach (var item in input.Items)
                {
                    counter = counter + 1;
                    var invoiceItem = new LineItem();

                    invoiceItem.Quantity = Convert.ToDecimal(item.Quantity);
                    invoiceItem.UnitPrice = Convert.ToDecimal(item.UnitPrice);
                    invoiceItem.DiscountPercentage = Convert.ToDecimal(item.DiscountPercentage);

                    decimal costPrice = invoiceItem.Quantity * invoiceItem.UnitPrice;
                    if (invoiceItem.DiscountPercentage != 0)
                        invoiceItem.DiscountAmount = costPrice * (invoiceItem.DiscountPercentage / 100);
                    else
                        invoiceItem.DiscountAmount = 0;

                    invoiceItem.VatRate = Convert.ToDecimal(item.VATRate);
                    invoiceItem.VatAmount = (costPrice - invoiceItem.DiscountAmount) * (invoiceItem.VatRate / 100);
                    if (taxTotal == 0)
                        taxTotal = invoiceItem.VatAmount;
                    else
                        taxTotal = taxTotal + invoiceItem.VatAmount;

                    invoiceItem.GrossPrice = costPrice - invoiceItem.DiscountAmount;
                    if (price == 0)
                        price = invoiceItem.GrossPrice;
                    else
                        price = price + invoiceItem.GrossPrice;

                    invoiceItem.NetPrice = invoiceItem.GrossPrice + invoiceItem.VatAmount;
                    if (total == 0)
                        total = invoiceItem.NetPrice;
                    else
                        total = total + invoiceItem.NetPrice;

                    discount += invoiceItem.DiscountAmount; invItems.Add(item);


                }

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                if (String.IsNullOrEmpty(input.VATDetails[0].VATCode) && total < 1000 && countryCode == "SA")
                {
                    type = "02";
                    invoiceType = "simplified";
                }
                else
                {
                    type = "01";
                    invoiceType = "standard";
                }
                builder.Append(type);
                builder.Append(request.ThirdParty ? "1" : "0");
                builder.Append(request.Nominal ? "1" : "0");
                builder.Append(request.Export ? "1" : "0");
                builder.Append(request.Summary ? "1" : "0");
                builder.Append(request.ThirdParty ? "1" : "0");
                request.InvoiceTransactionType = builder.ToString();

                XmlElement extensionContent = @"
                                    <sig:UBLDocumentSignatures   xmlns:cac= ""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"" xmlns:cbc= ""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"" xmlns:ext= ""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"" xmlns:sig=""urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2"" xmlns:sac=""urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2"" xmlns:sbc=""urn:oasis:names:specification:ubl:schema:xsd:SignatureBasicComponents-2"">
                <sac:SignatureInformation>
                    <cbc:ID>urn:oasis:names:specification:ubl:signature:1</cbc:ID>
                    <sbc:ReferencedSignatureID>urn:oasis:names:specification:ubl:signature:Invoice</sbc:ReferencedSignatureID>
                    <ds:Signature xmlns:ds=""http://www.w3.org/2000/09/xmldsig#"" Id=""signature"">
                        <ds:SignedInfo>
                            <ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2006/12/xml-c14n11""/>
                            <ds:SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256""/>
                            <ds:Reference Id=""invoiceSignedData"" URI="""">
                                <ds:Transforms>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::ext:UBLExtensions)</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::cac:Signature)</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116"">
                                        <ds:XPath>not(//ancestor-or-self::cac:AdditionalDocumentReference[cbc:ID='QR'])</ds:XPath>
                                    </ds:Transform>
                                    <ds:Transform Algorithm=""http://www.w3.org/2006/12/xml-c14n11""/>
                                </ds:Transforms>
                                <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                <ds:DigestValue>PEx8bNFcEMEpHzUVvQntQI6ot8eFqTT/l59b+H1HqX4=</ds:DigestValue>
                            </ds:Reference>
                            <ds:Reference Type=""http://www.w3.org/2000/09/xmldsig#SignatureProperties"" URI=""#xadesSignedProperties"">
                                <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                <ds:DigestValue>ZDEyMDUyODJjYzk4MGViNTJhNmYzMGIyZTgxODhkY2JlOWEzNmRiMTFlZTVhMDAxNjk5OTRkYTg3ODhlY2ZiMw==</ds:DigestValue>
                            </ds:Reference>
                        </ds:SignedInfo>
                        <ds:SignatureValue>MEUCIQC90fFYOqTimHvYP1f9bbT5stAfR8bI2fAAFAzYAvMCPQIgcGpGhMSocxfwdvcSW1B1523g5nD8bCe8SCWNect5rKM=</ds:SignatureValue>
                        <ds:KeyInfo>
                            <ds:X509Data>
                                <ds:X509Certificate>MIID6TCCA5CgAwIBAgITbwAAf8tem6jngr16DwABAAB/yzAKBggqhkjOPQQDAjBjMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxEzARBgoJkiaJk/IsZAEZFgNnb3YxFzAVBgoJkiaJk/IsZAEZFgdleHRnYXp0MRwwGgYDVQQDExNUU1pFSU5WT0lDRS1TdWJDQS0xMB4XDTIyMDkxNDEzMjYwNFoXDTI0MDkxMzEzMjYwNFowTjELMAkGA1UEBhMCU0ExEzARBgNVBAoTCjMxMTExMTExMTExDDAKBgNVBAsTA1RTVDEcMBoGA1UEAxMTVFNULTMxMTExMTExMTEwMTExMzBWMBAGByqGSM49AgEGBSuBBAAKA0IABGGDDKDmhWAITDv7LXqLX2cmr6+qddUkpcLCvWs5rC2O29W/hS4ajAK4Qdnahym6MaijX75Cg3j4aao7ouYXJ9GjggI5MIICNTCBmgYDVR0RBIGSMIGPpIGMMIGJMTswOQYDVQQEDDIxLVRTVHwyLVRTVHwzLWE4NjZiMTQyLWFjOWMtNDI0MS1iZjhlLTdmNzg3YTI2MmNlMjEfMB0GCgmSJomT8ixkAQEMDzMxMTExMTExMTEwMTExMzENMAsGA1UEDAwEMTEwMDEMMAoGA1UEGgwDVFNUMQwwCgYDVQQPDANUU1QwHQYDVR0OBBYEFDuWYlOzWpFN3no1WtyNktQdrA8JMB8GA1UdIwQYMBaAFHZgjPsGoKxnVzWdz5qspyuZNbUvME4GA1UdHwRHMEUwQ6BBoD+GPWh0dHA6Ly90c3RjcmwuemF0Y2EuZ292LnNhL0NlcnRFbnJvbGwvVFNaRUlOVk9JQ0UtU3ViQ0EtMS5jcmwwga0GCCsGAQUFBwEBBIGgMIGdMG4GCCsGAQUFBzABhmJodHRwOi8vdHN0Y3JsLnphdGNhLmdvdi5zYS9DZXJ0RW5yb2xsL1RTWkVpbnZvaWNlU0NBMS5leHRnYXp0Lmdvdi5sb2NhbF9UU1pFSU5WT0lDRS1TdWJDQS0xKDEpLmNydDArBggrBgEFBQcwAYYfaHR0cDovL3RzdGNybC56YXRjYS5nb3Yuc2Evb2NzcDAOBgNVHQ8BAf8EBAMCB4AwHQYDVR0lBBYwFAYIKwYBBQUHAwIGCCsGAQUFBwMDMCcGCSsGAQQBgjcVCgQaMBgwCgYIKwYBBQUHAwIwCgYIKwYBBQUHAwMwCgYIKoZIzj0EAwIDRwAwRAIgOgjNPJW017lsIijmVQVkP7GzFO2KQKd9GHaukLgIWFsCIFJF9uwKhTMxDjWbN+1awsnFI7RLBRxA/6hZ+F1wtaqU</ds:X509Certificate>
                            </ds:X509Data>
                        </ds:KeyInfo>
                        <ds:Object>
                            <xades:QualifyingProperties xmlns:xades=""http://uri.etsi.org/01903/v1.3.2#"" Target=""signature"">
                                <xades:SignedProperties Id=""xadesSignedProperties"">
                                    <xades:SignedSignatureProperties>
                                        <xades:SigningTime>2023-01-11T13:08:10Z</xades:SigningTime>
                                        <xades:SigningCertificate>
                                            <xades:Cert>
                                                <xades:CertDigest>
                                                    <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
                                                    <ds:DigestValue>YTJkM2JhYTcwZTBhZTAxOGYwODMyNzY3NTdkZDM3YzhjY2IxOTIyZDZhM2RlZGJiMGY0NDUzZWJhYWI4MDhmYg==</ds:DigestValue>
                                                </xades:CertDigest>
                                                <xades:IssuerSerial>
                                                    <ds:X509IssuerName>CN=TSZEINVOICE-SubCA-1, DC=extgazt, DC=gov, DC=local</ds:X509IssuerName>
                                                    <ds:X509SerialNumber>2475382886904809774818644480820936050208702411</ds:X509SerialNumber>
                                                </xades:IssuerSerial>
                                            </xades:Cert>
                                        </xades:SigningCertificate>
                                    </xades:SignedSignatureProperties>
                                </xades:SignedProperties>
                            </xades:QualifyingProperties>
                        </ds:Object>
                    </ds:Signature>
                </sac:SignatureInformation>
            </sig:UBLDocumentSignatures>
                               ".ToXmlElement();
                var doc = new InvoiceType();



                doc = new InvoiceType
                {
                    UBLExtensions = new List<UBLExtensionType>()
                      {
                      new UBLExtensionType
                      {

                        ExtensionURI="urn:oasis:names:specification:ubl:dsig:enveloped:xades",
                        ExtensionContent=extensionContent
                      }
                    },
                    ProfileID = "reporting:1.0",
                    ID = invoiceno.ToString(),
                    UUID = uniqueIdentifier.ToString(),
                    IssueDate = new DateType
                    {
                        Value = input.IssueDate //to be checked
                    },
                    IssueTime = new TimeType
                    {
                        Value = input.IssueDate //to be checked
                    },
                    InvoiceTypeCode = new CodeType
                    {
                        name = request.InvoiceTransactionType,
                        Value = "388"  //Invoice type specific
                    },
                    DocumentCurrencyCode = new CodeType
                    {
                        Value = "SAR"
                    },
                    TaxCurrencyCode = new CodeType
                    {
                        Value = "SAR"
                    },
                    LineCountNumeric = new NumericType
                    {
                        Value = invItems.Count
                    },
                    AdditionalDocumentReference = new List<DocumentReferenceType>()
                            {
                         new DocumentReferenceType
                                {
                                    ID = "ICV",
                                    UUID= invoiceno.ToString(),
                                },
                                new DocumentReferenceType
                                {
                                    ID = "PIH",
                                    Attachment = new AttachmentType
                                    {
                                        EmbeddedDocumentBinaryObject = new BinaryObjectType
                                        {
                                            mimeCode = "text/plain",
                                            Value = Convert.FromBase64String("NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==")
                                        }
                                    }
                                },

                                new DocumentReferenceType
                                {
                                    ID = "QR",
                                    Attachment = new AttachmentType
                                    {
                                        EmbeddedDocumentBinaryObject = new BinaryObjectType
                                        {
                                            mimeCode = "text/plain",
                                            Value = Convert.FromBase64String(qrString)
                                        }
                                    }
                                }
                            },
                    Signature = new List<SignatureType>()
                            {
                              new SignatureType
                              {
                                ID="urn:oasis:names:specification:ubl:signature:Invoice",
                                SignatureMethod="urn:oasis:names:specification:ubl:dsig:enveloped:xades"
                              }
                            },
                    AccountingSupplierParty = new SupplierPartyType
                    {
                        Party = new PartyType
                        {
                            PartyIdentification = new List<PartyIdentificationType>()
                                {
                                    new PartyIdentificationType
                                    {
                                        ID = new IdentifierType
                                        {
                                            schemeID = "CRN",
                                            Value = input.Supplier.CRNumber
                                        }
                                    }
                                },

                            PostalAddress = new AddressType
                            {
                                StreetName = input.Supplier.Address.Street ?? "",
                                BuildingNumber = input.Supplier.Address.BuildingNo ?? "",
                                PlotIdentification = input.Supplier.Address.AdditionalNo ?? "",
                                CitySubdivisionName = input.Supplier.Address.AdditionalStreet ?? "",
                                CityName = input.Supplier.Address.City ?? "",
                                PostalZone = input.Supplier.Address.PostalCode ?? "",
                                CountrySubentity = input.Supplier.Address.State ?? "",
                                Country = new CountryType
                                {
                                    IdentificationCode = new CodeType
                                    {
                                        Value = input.Supplier.Address.CountryCode ?? ""
                                    }
                                }
                            },
                            PartyTaxScheme = new List<PartyTaxSchemeType>()
                                    {
                                        new PartyTaxSchemeType
                                        {
                                            CompanyID = new IdentifierType
                                            {
                                                  Value = input.Supplier.VATID
                                            },
                                            TaxScheme = new TaxSchemeType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "VAT"
                                                }
                                            }
                                        }
                                    },
                            PartyLegalEntity = new List<PartyLegalEntityType>()
                                    {
                                        new PartyLegalEntityType
                                        {
                                            RegistrationName = input.Supplier.RegistrationName
                                        }
                                    }
                        }
                    },
                    AccountingCustomerParty = new CustomerPartyType
                    {
                        Party = new PartyType
                        {
                            PartyIdentification = new List<PartyIdentificationType>()
                                {
                                    new PartyIdentificationType
                                    {
                                        ID = new IdentifierType
                                        {
                                            schemeID = "NAT",
                                            Value = "2345" //need information
                                        }
                                    }
                                },
                            //Person = new List<PersonType>()
                            //    {
                            //        new PersonType{
                            //        Contact = new ContactType
                            //        {
                            //            ElectronicMail = input.Buyer.ContactPerson.Email,
                            //            Telephone = input.Buyer.ContactPerson.ContactNumber,
                            //            Name = input.Buyer.ContactPerson.Name

                            //        }       //recheck
                            //        }
                            //    },

                            PostalAddress = new AddressType
                            {
                                StreetName = input.Buyer.Address.Street ?? "",
                                BuildingNumber = input.Buyer.Address.BuildingNo ?? "",
                                PlotIdentification = input.Buyer.Address.AdditionalNo ?? "",
                                CitySubdivisionName = input.Buyer.Address.AdditionalStreet ?? "",
                                CityName = input.Buyer.Address.City ?? "",
                                PostalZone = input.Buyer.Address.PostalCode ?? "",
                                CountrySubentity = input.Buyer.Address.State ?? "",
                                Country = new CountryType
                                {
                                    IdentificationCode = new CodeType
                                    {
                                        Value = input.Supplier.Address.CountryCode
                                    }
                                }
                            },
                            PartyTaxScheme = new List<PartyTaxSchemeType>()
                                    {
                                        new PartyTaxSchemeType
                                        {
                                            TaxScheme = new TaxSchemeType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "VAT"
                                                }
                                            },
                                            CompanyID = new IdentifierType
                                            {
                                                Value=input.Buyer.VATID
                                            }
                                        }
                                    },
                            PartyLegalEntity = new List<PartyLegalEntityType>()
                                    {
                                        new PartyLegalEntityType
                                        {
                                           RegistrationName = input.Supplier.RegistrationName
                                        }
                                    }
                        }
                    },
                    //Delivery = new List<DeliveryType>()
                    //{
                    //    new DeliveryType
                    //    {
                    //        ActualDeliveryDate = input.LatestDeliveryDate
                    //    }
                    //},
                    PaymentMeans = new List<PaymentMeansType>()
                              {
                                new PaymentMeansType
                                {
                                    PaymentMeansCode = new CodeType
                                    {
                                        Value = "42" //need information
                                    }
                                }
                              },
                    AllowanceCharge = new List<AllowanceChargeType>()
                    {
                        new AllowanceChargeType
                        {
                            ID = new IdentifierType
                            {
                                Value="1"
                            },
                            ChargeIndicator = new IndicatorType
                            {
                                Value=false
                            },
                            AllowanceChargeReason = new List<TextType>()
                            {
                                new TextType()
                                {
                                    Value = "discount"
                                }
                            },
                            Amount = new AmountType
                            {
                                currencyID="SAR",
                                Value= Math.Round(discount,2)
                            },
                            TaxCategory = new List<TaxCategoryType>()
                            {
                                new TaxCategoryType
                                        {
                                                        ID = new IdentifierType
                                                        {
                                                            schemeAgencyID="6",
                                                            schemeID="UN/ECE 5305",  //need information
                                                            Value = "S"
                                                        },
                                                        Percent = 15M,
                                                        TaxScheme = new TaxSchemeType
                                                        {
                                                            ID = new IdentifierType
                                                            {
                                                                schemeAgencyID="6",
                                                                schemeID="UN/ECE 5153",  //need information
                                                                Value = "VAT",
                                                            }
                                                        }
                                                    }
                            }
                        }
                    },
                    TaxTotal = new List<TaxTotalType>()
                                   {
                                        new TaxTotalType
                                        {
                                            TaxAmount = new AmountType
                                            {
                                                currencyID = "SAR",
                                                Value =Math.Round(taxTotal,2) //135.00M
                                            },

                                            TaxSubtotal = new List<TaxSubtotalType>()
                                            {
                                                new TaxSubtotalType
                                                {
                                                    TaxableAmount = new AmountType
                                                    {
                                                        currencyID = "SAR",
                                                        Value =Math.Round(price,2)  //90Math.Round(discount,2) 
                                                    },
                                                    TaxAmount = new AmountType
                                                    {
                                                        currencyID = "SAR",
                                                        Value = Math.Round(taxTotal,2) //135.00M 
                                                    },
                                                    TaxCategory = new TaxCategoryType
                                                    {
                                                        ID = new IdentifierType
                                                        {
                                                            schemeAgencyID="6",
                                                            schemeID="UN/ECE 5305",  //need information
                                                            Value = "S"
                                                        },
                                                        Percent = 15M,
                                                        TaxScheme = new TaxSchemeType
                                                        {
                                                            ID = new IdentifierType
                                                            {
                                                                schemeAgencyID="6",
                                                                schemeID="UN/ECE 5153",  //need information
                                                                Value = "VAT",
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new TaxTotalType
                                        {
                                            TaxAmount=new AmountType
                                            {
                                                currencyID="SAR",
                                                Value=Math.Round(taxTotal,2)
                                            }
                                        }
                                   },
                    LegalMonetaryTotal = new MonetaryTotalType
                    {
                        LineExtensionAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(price + discount, 2)
                        },
                        TaxExclusiveAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(price, 2)
                        },
                        TaxInclusiveAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(total, 2)
                        },
                        AllowanceTotalAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(discount, 2)
                        },
                        PayableAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(total, 2)
                        }
                    },
                    InvoiceLine = new List<InvoiceLineType>()

                };

                var x = 0;
                foreach (var item in invItems)
                {
                    x++;
                    doc.InvoiceLine.Add(

                        new InvoiceLineType
                        {
                            ID = x.ToString(),
                            InvoicedQuantity = new QuantityType
                            {
                                unitCode = "PCE", //need information
                                Value = item.Quantity
                            },
                            LineExtensionAmount = new AmountType
                            {
                                currencyID = "SAR",
                                Value = Math.Round(item.Quantity * item.UnitPrice - Convert.ToDecimal(item.DiscountAmount), 2)
                            },
                            TaxTotal = new List<TaxTotalType>()
                                {
                                        new TaxTotalType
                                        {
                                            TaxAmount = new AmountType
                                            {
                                                currencyID = "SAR",
                                                Value = Math.Round(item.VATAmount,2)
                                            },
                                            RoundingAmount = new AmountType
                                            {
                                              currencyID= "SAR",
                                              Value=Math.Round(item.LineAmountInclusiveVAT,2)
                                            }
                                        }
                                },
                            Item = new ItemType

                            {
                                Name = item.Name + " " + item.Description,

                                ClassifiedTaxCategory = new List<TaxCategoryType>()
                                    {
                                            new TaxCategoryType
                                            {
                                                ID = new IdentifierType
                                                {
                                                    Value = "S"
                                                },
                                                Percent = 15M,
                                                TaxScheme = new TaxSchemeType
                                                {
                                                    ID = new IdentifierType
                                                    {
                                                        Value = "VAT"
                                                    }
                                                }
                                            }
                                    }
                            },
                            Price = new PriceType
                            {
                                PriceAmount = new AmountType
                                {
                                    currencyID = "SAR",
                                    Value = Math.Round(item.UnitPrice, 2)
                                },
                                //BaseQuantity = new QuantityType
                                //{
                                //    unitCode = "PCE",
                                //    Value = item.Quantity
                                //},
                                AllowanceCharge = new List<AllowanceChargeType>
                                    {
                                            new AllowanceChargeType {
                                                 ChargeIndicator=false,
                                           //      MultiplierFactorNumeric = item.DiscountPercentage,
                                        Amount=new AmountType
                                        {
                                            currencyID="SAR",
                                            Value = Math.Round( Convert.ToDecimal(item.DiscountAmount),2)
                                        },
                                        AllowanceChargeReason=new List<TextType>()
                                        {
                                            new TextType
                                            {
                                                Value="discount"
                                            }
                                        }
                                        //BaseAmount = new AmountType
                                        //{
                                        //    currencyID="SAR",
                                        //    Value = Math.Round(item.UnitPrice,2)
                                        //}
                                       }
                                  }
                            }


                        });
                }

                doc.Xmlns = new System.Xml.Serialization.XmlSerializerNamespaces(new[] {
                            new XmlQualifiedName(null,"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"),
                            new XmlQualifiedName("cac","urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"),
                            new XmlQualifiedName("cbc","urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"),
                            new XmlQualifiedName("ext","urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")
                        });


                var pathToSave = string.Empty;
                if (tenantId != null && tenantId != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, uniqueIdentifier);
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0", uniqueIdentifier);
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("/", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
                var path = (Path.Combine(pathToSave, xmlfileName));

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(InvoiceType));
                var xml = "";

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, doc, doc.Xmlns);
                    xml = textWriter.ToString();
                }

                await SignXml(new XMLSignInput()
                {
                    xml = xml,
                    UUID = input.Additional_Info,
                    qrPath = Path.Combine(pathToSave, uniqueIdentifier + "_" + invoiceno + ".png"),
                    SellerName = input.Supplier.RegistrationName,
                    VatId = input.Supplier.VATID,
                    TotalAmount = input.InvoiceSummary.TotalAmountWithVAT,
                    VatAmount = input.InvoiceSummary.TotalVATAmount,
                    xmlPath = path,
                    pathToSave = pathToSave,
                    xmlModel = doc
                });

                return true;

            }

            catch (Exception ex)
            {
                return false;
            }
        


        }

    }

}
