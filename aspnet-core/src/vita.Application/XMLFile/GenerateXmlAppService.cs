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
using SignXML.Models.API;
using System.Linq;
using LineItem = vita.UblSharp.Dtos.LineItem;
using vita.EInvoicing.Dto;
using AutoMapper;
using vita.Credit;
using Abp.UI;
using System.Security.Policy;
using NPOI.SS.Formula.Functions;
using System.Runtime.ConstrainedExecution;
using vita.Sales;

namespace vita.UblSharp
{
    public class GenerateXmlAppService : vitaAppServiceBase,IGenerateXmlAppService
    {
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly IMapper mapper;

        public GenerateXmlAppService(IDbContextProvider<vitaDbContext> dbContextProvider, IMapper mapper)
        {
            _dbContextProvider = dbContextProvider;
            this.mapper = mapper;
        }


        public class Phase2RequestDto
        {
            public XMLSignInput xmlInput { get; set; }

            public ComplianceAPISuccess Success { get; set; }

            public ZatcaDigitalSignature  sign { get; set; }
        }
        public async Task<bool> InsertUploadDatatoLogs(string text, string date)
        {

            try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "InsertIntoLogs";
                        cmd.Parameters.AddWithValue("@text", text);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        int i = cmd.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    }
                }
                return true;
            }
            catch (Exception e)
            {

                return false;

            }
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
                        cmd.Parameters.AddWithValue("@tenantId", input.tenantId ?? 0);
                        cmd.Parameters.AddWithValue("@signature", input.signature);
                        cmd.Parameters.AddWithValue("@certificate", input.certificate);
                        cmd.Parameters.AddWithValue("@xml64", input.xml64);
                        cmd.Parameters.AddWithValue("@invoiceHash64", input.invoiceHash64);
                        cmd.Parameters.AddWithValue("@csid", input.csid);
                        cmd.Parameters.AddWithValue("@qrBase64", input.qrBase64);
                        cmd.Parameters.AddWithValue("@complianceInvoiceResponse", input.complianceInvoiceResponse);
                        cmd.Parameters.AddWithValue("@reportInvoiceResponse", input.reportInvoiceResponse);
                        cmd.Parameters.AddWithValue("@clearanceResponse", input.clearanceResponse);
                        cmd.Parameters.AddWithValue("@totalAmount", input.totalAmount);
                        cmd.Parameters.AddWithValue("@vatAmount", input.vatAmount);
                        cmd.Parameters.AddWithValue("@irnno", input.irnno);
                        cmd.Parameters.AddWithValue("@errors", input.errors);
                        cmd.Parameters.AddWithValue("@status", input.status);

                        var reader = cmd.ExecuteNonQuery();

                    }

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());

            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }
        }

        public async Task<(ZatcaComplianceCSIDResponse,ZatcaDigitalSignature)> GetCSID(string otp)
       {
            try
            {
                Zatca.ConfigureOpenSSL(new OpenSSLConfig()
                {
                    useEnvironmentVariable = false,
                    useAssemblyEntryPoint = true,
                    pathToOpenSSL = "SignXML\\openssl\\binn"
                });

                Zatca.ConfigureCertConf(new CertificateConfig()
                {
                    useEnvironmentDirectory = false,
                    useAssemblyEntryPoint = true,
                    pathToConfig = "SignXML\\Configuration.cnf"
                });

                var signature = Zatca.GetDigitalSignature("");
                var csr = Zatca.GetCSR(signature.EcdsaPrivateKey, null);
                InsertUploadDatatoLogs(("CSR")+( JsonConvert.SerializeObject(csr)), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));


                var csid = await Zatca.GetCSIDAsync(csr.CertificateRequestBase64, otp);

                //try at least thrice on failure
                var i = 0;
                while (csid.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    csid = await Zatca.GetCSIDAsync(csr.CertificateRequestBase64, otp);
                    i++;
                    if (i > 3)
                    {
                        throw new UserFriendlyException("Failed to generate CSID");
                    }
                };
                InsertUploadDatatoLogs(("CSID")+ (JsonConvert.SerializeObject(csid)), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

                return (csid,signature);

            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Failed to generate CSID");
            }
        }

        public async Task<(ZatcaReportingResponse,ZatcaComplianceInvoiceResponse,ZatcaClearanceResponse)> SendToZatca(Phase2RequestDto phase2RequestDto)
        {
            var xmlst = phase2RequestDto.xmlInput.xml;
            var input = phase2RequestDto.xmlInput;
            var csid = new ZatcaComplianceCSIDResponse()
            {
                Error=null,
                Success = phase2RequestDto.Success,
                StatusCode = System.Net.HttpStatusCode.OK
            };
            var s = phase2RequestDto.sign;
            try
            {
                Zatca.ConfigureOpenSSL(new OpenSSLConfig()
                {
                    useEnvironmentVariable = false,
                    useAssemblyEntryPoint = true,
                    pathToOpenSSL = "SignXML\\openssl\\binn"
                });

                Zatca.ConfigureCertConf(new CertificateConfig()
                {
                    useEnvironmentDirectory = false,
                    useAssemblyEntryPoint = true,
                    pathToConfig = "SignXML\\Configuration.cnf"
                });

                var xml = input.xml;


                var hash = Zatca.GetInvoiceHash(xml);

                var signature = Zatca.GetDigitalSignature(hash.InvoiceHash,s.EcdsaPrivateKey);

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


                var qrBase64 = Zatca.GetQrCode(new ZatcaQrCode()
                {
                    SellerName = input.SellerName,
                    VatNumber = input.VatId,
                    IssueDateTime = input.IssueDate,
                    TotalAmount = Math.Round(1.00m * input.TotalAmount, 2),//input.TotalAmount,
                    VatAmount = Math.Round(1.00m * input.VatAmount, 2),//input.VatAmount,
                    XmlHash = hash.InvoiceHashBase64,
                    Signature = signature.SignatureBase64,
                    PublicKey = certificate.PublicKey,
                    PublicKeySignature = certificate.Signature,
                    isSimplified = input.TotalAmount < 1000
                },null, input.isPhase1); // set isPhase1 parameter here (default is true)

                  xml = Zatca.AddQRToXML(xml, qrBase64);

                InsertUploadDatatoLogs(("Modified XML")+(JsonConvert.SerializeObject(xml)), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));


                xml = xmlst;

                InsertUploadDatatoLogs("Original Xml", JsonConvert.SerializeObject(xml));


                var complianceInvoice = await Zatca.ComplianceInvoiceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));

                InsertUploadDatatoLogs(("Compliance Invoice")+(JsonConvert.SerializeObject(complianceInvoice)), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));


                int i = 0;
                if (complianceInvoice.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    i = 0;
                   // csid.Success.requestId = "1234567890123";
                    csid = await Zatca.GetOnboardingProductionCSIDAsync(csid);

                    while (csid?.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        csid = await Zatca.GetOnboardingProductionCSIDAsync(csid);
                        i++;
                        if (i > 3)
                        {
                            throw new Exception();
                        }
                    }
                    InsertUploadDatatoLogs(("Prod CSID")+(JsonConvert.SerializeObject(csid)), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

                }


                ZatcaReportingResponse reportingRes = null;
                ZatcaClearanceResponse clearanceResponse = null;

                csid.Success.binarySecurityToken = "TUlJRDFEQ0NBM21nQXdJQkFnSVRid0FBZTNVQVlWVTM0SS8rNVFBQkFBQjdkVEFLQmdncWhrak9QUVFEQWpCak1SVXdFd1lLQ1pJbWlaUHlMR1FCR1JZRmJHOWpZV3d4RXpBUkJnb0praWFKay9Jc1pBRVpGZ05uYjNZeEZ6QVZCZ29Ka2lhSmsvSXNaQUVaRmdkbGVIUm5ZWHAwTVJ3d0dnWURWUVFERXhOVVUxcEZTVTVXVDBsRFJTMVRkV0pEUVMweE1CNFhEVEl5TURZeE1qRTNOREExTWxvWERUSTBNRFl4TVRFM05EQTFNbG93U1RFTE1Ba0dBMVVFQmhNQ1UwRXhEakFNQmdOVkJBb1RCV0ZuYVd4bE1SWXdGQVlEVlFRTEV3MW9ZWGxoSUhsaFoyaHRiM1Z5TVJJd0VBWURWUVFERXdreE1qY3VNQzR3TGpFd1ZqQVFCZ2NxaGtqT1BRSUJCZ1VyZ1FRQUNnTkNBQVRUQUs5bHJUVmtvOXJrcTZaWWNjOUhEUlpQNGI5UzR6QTRLbTdZWEorc25UVmhMa3pVMEhzbVNYOVVuOGpEaFJUT0hES2FmdDhDL3V1VVk5MzR2dU1ObzRJQ0p6Q0NBaU13Z1lnR0ExVWRFUVNCZ0RCK3BId3dlakViTUJrR0ExVUVCQXdTTVMxb1lYbGhmREl0TWpNMGZETXRNVEV5TVI4d0hRWUtDWkltaVpQeUxHUUJBUXdQTXpBd01EYzFOVGc0TnpBd01EQXpNUTB3Q3dZRFZRUU1EQVF4TVRBd01SRXdEd1lEVlFRYURBaGFZWFJqWVNBeE1qRVlNQllHQTFVRUR3d1BSbTl2WkNCQ2RYTnphVzVsYzNNek1CMEdBMVVkRGdRV0JCU2dtSVdENmJQZmJiS2ttVHdPSlJYdkliSDlIakFmQmdOVkhTTUVHREFXZ0JSMllJejdCcUNzWjFjMW5jK2FyS2NybVRXMUx6Qk9CZ05WSFI4RVJ6QkZNRU9nUWFBL2hqMW9kSFJ3T2k4dmRITjBZM0pzTG5waGRHTmhMbWR2ZGk1ellTOURaWEowUlc1eWIyeHNMMVJUV2tWSlRsWlBTVU5GTFZOMVlrTkJMVEV1WTNKc01JR3RCZ2dyQmdFRkJRY0JBUVNCb0RDQm5UQnVCZ2dyQmdFRkJRY3dBWVppYUhSMGNEb3ZMM1J6ZEdOeWJDNTZZWFJqWVM1bmIzWXVjMkV2UTJWeWRFVnVjbTlzYkM5VVUxcEZhVzUyYjJsalpWTkRRVEV1WlhoMFoyRjZkQzVuYjNZdWJHOWpZV3hmVkZOYVJVbE9WazlKUTBVdFUzVmlRMEV0TVNneEtTNWpjblF3S3dZSUt3WUJCUVVITUFHR0gyaDBkSEE2THk5MGMzUmpjbXd1ZW1GMFkyRXVaMjkyTG5OaEwyOWpjM0F3RGdZRFZSMFBBUUgvQkFRREFnZUFNQjBHQTFVZEpRUVdNQlFHQ0NzR0FRVUZCd01DQmdnckJnRUZCUWNEQXpBbkJna3JCZ0VFQVlJM0ZRb0VHakFZTUFvR0NDc0dBUVVGQndNQ01Bb0dDQ3NHQVFVRkJ3TURNQW9HQ0NxR1NNNDlCQU1DQTBrQU1FWUNJUUNWd0RNY3E2UE8rTWNtc0JYVXovdjFHZGhHcDdycVNhMkF4VEtTdjgzOElBSWhBT0JOREJ0OSszRFNsaWpvVmZ4enJkRGg1MjhXQzM3c21FZG9HV1ZyU3BHMQ==";
                csid.Success.secret = "Xlj15LyMCgSC66ObnEO/qVPfhSbs3kDTjWnGheYhfSs=";

                if (!input.isPhase1)
                {
                    if (input.TotalAmount < 1000)
                    {
                        reportingRes = await Zatca.ReportInvoiceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));
                    }
                    else
                    {
                        clearanceResponse = await Zatca.ClearanceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));
                    }
                }

                InsertUploadDatatoLogs(("Reporting Responde")+(JsonConvert.SerializeObject(reportingRes)), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                InsertUploadDatatoLogs(("Clearence Responde")+(JsonConvert.SerializeObject(clearanceResponse)), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));


                return (reportingRes,complianceInvoice,clearanceResponse);
              
            }
            catch(Exception e)
            {
                throw new UserFriendlyException("Something went wrong");
            }
        }


        public async Task<XMLLogInput> SignXml(XMLSignInput input)
        {
            var log = new XMLLogInput() 
            { 
                uuid = input.xml_uuid, 
                tenantId = input.tenantId,
                totalAmount=input.TotalAmount,
                vatAmount=input.VatAmount,
                irnno = input.irnno,
                status =input.TotalAmount<1000? "NOT_REPORTED":"NOT_CLEARED"
            };
            try
            {
                Zatca.ConfigureOpenSSL(new OpenSSLConfig()
                {
                    useEnvironmentVariable = false,
                    useAssemblyEntryPoint = true,
                    pathToOpenSSL = "SignXML\\openssl\\binn"
                }); 

                Zatca.ConfigureCertConf(new CertificateConfig()
                {
                    useEnvironmentDirectory = false,
                    useAssemblyEntryPoint = true,
                    pathToConfig="SignXML\\Configuration.cnf"
                });

                var xml = Zatca.GetXMLFromCustomMapping(input.xmlModel);
                log.xml64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));


                var hash = Zatca.GetInvoiceHash(xml);
                log.invoiceHash64 = hash.InvoiceHashBase64;

                var signature = Zatca.GetDigitalSignature(hash.InvoiceHash);
                log.signature = JsonConvert.SerializeObject(signature);


                var csr = Zatca.GetCSR(signature.EcdsaPrivateKey, null);
     

                var csid = await Zatca.GetCSIDAsync(csr.CertificateRequestBase64, "123345");
                log.csid = JsonConvert.SerializeObject(csid);

                //try at least thrice on failure
                var i = 0;
                while (csid.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    csid = await Zatca.GetCSIDAsync(csr.CertificateRequestBase64, "123345");
                    log.csid = JsonConvert.SerializeObject(csid);
                    i++;
                    if (i > 3)
                    {
                        log.errors = "Failed to Get CSID";
                        return log;
                    }
                };



                var certificate = Zatca.GetCertificate(csid);
                log.certificate = JsonConvert.SerializeObject(certificate);


                xml = Zatca.AddSignedPropertiesToXML(xml, new ZatcaSignedProperties
                {
                    DigestValue = certificate.CertificateHash, //to be checked
                    SigningTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                    X509IssuerName = certificate.IssuerName,
                    X509SerialNumber = certificate.SerialNumber
                });
                log.xml64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));

                var signedHash = Zatca.GenerateSignedPropertiesHash(xml);
                
                xml = Zatca.AddUBLExtensionsToXML(new ZatcaUblExtensions
                {
                    xml = xml,
                    SignatureValue = signature.SignatureBase64,
                    X509Certificate = certificate.CertificatePEM.Replace("-----BEGIN CERTIFICATE-----\n", "").Replace("\n-----END CERTIFICATE-----", ""),
                    EncodedSignedPropertiesHash = signedHash,
                    EncodedInvoiceHash = hash.InvoiceHashBase64
                });
                log.xml64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));


                if (!Directory.Exists(input.pathToSave))
                    Directory.CreateDirectory(input.pathToSave);

                var qrBase64 = Zatca.GetQrCode(new ZatcaQrCode()
                {
                    SellerName = input.SellerName,
                    VatNumber = input.VatId,
                    IssueDateTime = input.IssueDate,
                    TotalAmount = Math.Round(1.00m * input.TotalAmount, 2),//input.TotalAmount,
                    VatAmount = Math.Round(1.00m * input.VatAmount, 2),//input.VatAmount,
                    XmlHash = hash.InvoiceHashBase64,
                    Signature = signature.SignatureBase64,
                    PublicKey = certificate.PublicKey,
                    PublicKeySignature = certificate.Signature,
                    isSimplified = input.TotalAmount<1000
                }, input.qrPath,input.isPhase1); // set isPhase1 parameter here (default is true)

                log.qrBase64 = qrBase64;


                xml = Zatca.AddQRToXML(xml, qrBase64);
                log.xml64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));



                File.WriteAllText(input.xmlPath, xml);

                var complianceInvoice = await Zatca.ComplianceInvoiceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));
                log.complianceInvoiceResponse = JsonConvert.SerializeObject(complianceInvoice);

                ZatcaReportingResponse reportingRes = null;
                ZatcaClearanceResponse clearanceResponse = null;

                //if (complianceInvoice.StatusCode == System.Net.HttpStatusCode.OK)
                //{
                //    i = 0;
                //    csid.Success.requestId = "1234567890123";
                //    csid = await Zatca.GetOnboardingProductionCSIDAsync(csid);

                //    while (csid?.StatusCode != System.Net.HttpStatusCode.OK)
                //    {
                //        csid = await Zatca.GetOnboardingProductionCSIDAsync(csid);
                //        i++;
                //        if (i > 3)
                //        {
                //            log.errors = "Failed to get production CSID";
                //            return log;
                //        }
                //    }
                //}
                //else
                //{
                //    log.errors = "Invoice is not Zatca compliant. Check Compliance Invoice Response. ";
                //    return log;
                //}

                csid.Success.binarySecurityToken = "TUlJRDFEQ0NBM21nQXdJQkFnSVRid0FBZTNVQVlWVTM0SS8rNVFBQkFBQjdkVEFLQmdncWhrak9QUVFEQWpCak1SVXdFd1lLQ1pJbWlaUHlMR1FCR1JZRmJHOWpZV3d4RXpBUkJnb0praWFKay9Jc1pBRVpGZ05uYjNZeEZ6QVZCZ29Ka2lhSmsvSXNaQUVaRmdkbGVIUm5ZWHAwTVJ3d0dnWURWUVFERXhOVVUxcEZTVTVXVDBsRFJTMVRkV0pEUVMweE1CNFhEVEl5TURZeE1qRTNOREExTWxvWERUSTBNRFl4TVRFM05EQTFNbG93U1RFTE1Ba0dBMVVFQmhNQ1UwRXhEakFNQmdOVkJBb1RCV0ZuYVd4bE1SWXdGQVlEVlFRTEV3MW9ZWGxoSUhsaFoyaHRiM1Z5TVJJd0VBWURWUVFERXdreE1qY3VNQzR3TGpFd1ZqQVFCZ2NxaGtqT1BRSUJCZ1VyZ1FRQUNnTkNBQVRUQUs5bHJUVmtvOXJrcTZaWWNjOUhEUlpQNGI5UzR6QTRLbTdZWEorc25UVmhMa3pVMEhzbVNYOVVuOGpEaFJUT0hES2FmdDhDL3V1VVk5MzR2dU1ObzRJQ0p6Q0NBaU13Z1lnR0ExVWRFUVNCZ0RCK3BId3dlakViTUJrR0ExVUVCQXdTTVMxb1lYbGhmREl0TWpNMGZETXRNVEV5TVI4d0hRWUtDWkltaVpQeUxHUUJBUXdQTXpBd01EYzFOVGc0TnpBd01EQXpNUTB3Q3dZRFZRUU1EQVF4TVRBd01SRXdEd1lEVlFRYURBaGFZWFJqWVNBeE1qRVlNQllHQTFVRUR3d1BSbTl2WkNCQ2RYTnphVzVsYzNNek1CMEdBMVVkRGdRV0JCU2dtSVdENmJQZmJiS2ttVHdPSlJYdkliSDlIakFmQmdOVkhTTUVHREFXZ0JSMllJejdCcUNzWjFjMW5jK2FyS2NybVRXMUx6Qk9CZ05WSFI4RVJ6QkZNRU9nUWFBL2hqMW9kSFJ3T2k4dmRITjBZM0pzTG5waGRHTmhMbWR2ZGk1ellTOURaWEowUlc1eWIyeHNMMVJUV2tWSlRsWlBTVU5GTFZOMVlrTkJMVEV1WTNKc01JR3RCZ2dyQmdFRkJRY0JBUVNCb0RDQm5UQnVCZ2dyQmdFRkJRY3dBWVppYUhSMGNEb3ZMM1J6ZEdOeWJDNTZZWFJqWVM1bmIzWXVjMkV2UTJWeWRFVnVjbTlzYkM5VVUxcEZhVzUyYjJsalpWTkRRVEV1WlhoMFoyRjZkQzVuYjNZdWJHOWpZV3hmVkZOYVJVbE9WazlKUTBVdFUzVmlRMEV0TVNneEtTNWpjblF3S3dZSUt3WUJCUVVITUFHR0gyaDBkSEE2THk5MGMzUmpjbXd1ZW1GMFkyRXVaMjkyTG5OaEwyOWpjM0F3RGdZRFZSMFBBUUgvQkFRREFnZUFNQjBHQTFVZEpRUVdNQlFHQ0NzR0FRVUZCd01DQmdnckJnRUZCUWNEQXpBbkJna3JCZ0VFQVlJM0ZRb0VHakFZTUFvR0NDc0dBUVVGQndNQ01Bb0dDQ3NHQVFVRkJ3TURNQW9HQ0NxR1NNNDlCQU1DQTBrQU1FWUNJUUNWd0RNY3E2UE8rTWNtc0JYVXovdjFHZGhHcDdycVNhMkF4VEtTdjgzOElBSWhBT0JOREJ0OSszRFNsaWpvVmZ4enJkRGg1MjhXQzM3c21FZG9HV1ZyU3BHMQ==";
                csid.Success.secret = "Xlj15LyMCgSC66ObnEO/qVPfhSbs3kDTjWnGheYhfSs=";

                if (!input.isPhase1)
                {
                    if (input.TotalAmount < 1000)
                    {
                        reportingRes = await Zatca.ReportInvoiceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));
                        log.status = reportingRes?.ReportingStatus ?? "NOT_REPORTED";
                    }
                    else
                    {
                        clearanceResponse = await Zatca.ClearanceAsync(csid, hash.InvoiceHashBase64, input.UUID, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));
                        log.status = clearanceResponse?.ClearanceStatus ?? "NOT_CLEARED";
                    }
                }
                else
                {
                    log.status = "CLEARED";
                }
                log.reportInvoiceResponse = JsonConvert.SerializeObject(reportingRes);
                log.clearanceResponse = JsonConvert.SerializeObject(clearanceResponse);

                if (clearanceResponse?.ClearedInvoice != null )
                    {
                    log.xml64 = clearanceResponse.ClearedInvoice;
                    File.WriteAllText(input.xmlPath, Encoding.UTF8.GetString(Convert.FromBase64String(clearanceResponse.ClearedInvoice)));
                    }

            }
            catch(Exception e)
            {
                log.errors += e.ToString();
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return log;
          
        }


        private List<TaxTotalType> UpdateTaxTotal(List<InvoiceVATDetailDto> vatDetails)
        {
            List<TaxTotalType> li = new List<TaxTotalType>();
            List<TaxSubtotalType> TaxSubtotal = new List<TaxSubtotalType>();
            var TaxTotalType = new TaxTotalType
            {
                TaxAmount = new AmountType
                {
                    currencyID = "SAR",
                    Value = Math.Round(1.00m * vatDetails.Sum(a => a.TaxAmount), 2)
                },
                   TaxSubtotal = TaxSubtotal
            };

            TaxSubtotalType taxSubtotalType = new TaxSubtotalType();

            foreach (var vatDetail in vatDetails) {


                taxSubtotalType =
                                                new TaxSubtotalType
                                                {
                                                    TaxableAmount = new AmountType
                                                    {
                                                        currencyID = "SAR",
                                                        Value = Math.Round(1.00m * vatDetail.TaxableAmount, 2)
                                                    },
                                                    TaxAmount = new AmountType
                                                    {
                                                        currencyID = "SAR",
                                                        Value = Math.Round(1.00m * vatDetail.TaxAmount, 2)
                                                    },
                                                    TaxCategory = new TaxCategoryType
                                                    {
                                                        ID = new IdentifierType
                                                        {
                                                            schemeAgencyID = "6",
                                                            schemeID = "UN/ECE 5305",  //need information
                                                            Value = vatDetail.VATCode
                                                        },
                                                        Percent = vatDetail.VATRate,
                                                        TaxScheme = new TaxSchemeType
                                                        {
                                                            ID = new IdentifierType
                                                            {
                                                                schemeAgencyID = "6",
                                                                schemeID = "UN/ECE 5153",  //need information
                                                                Value = "VAT",
                                                            }
                                                        }
                                                    }
                                                };
                if (vatDetail.VATCode == "Z")
                {
                    taxSubtotalType.TaxCategory.TaxExemptionReasonCode = new CodeType
                    {
                        Value = vatDetail.ExcemptionReasonCode ?? "VATEX-SA-32",
                    };

                    taxSubtotalType.TaxCategory.TaxExemptionReason = new List<TextType> { new TextType
                        {
                            Value =  vatDetail.ExcemptionReasonText ?? "Zero rated"
                        } }
                   ;
                }

                TaxSubtotal.Add(taxSubtotalType);

            }

            TaxTotalType.TaxSubtotal = TaxSubtotal;

            li.Add(TaxTotalType);

            return li;
        }



        private string GetInvoiceTypeCode(InvoiceTypeEnum code)
        {
            if (code == InvoiceTypeEnum.Sales)
            {
                return "388";
            }
            else if (code == InvoiceTypeEnum.Credit)
            {
                return "381";
            }
            else if (code == InvoiceTypeEnum.Debit)
            {
                return "383";
            }
            else return null;
        }

        public async Task<bool> GenerateXmlRequest<T>(T inputGen,XMLRequestParam param) 
        {
            InvoiceRequest input = new InvoiceRequest();
            try
            {
                input = mapper.Map<InvoiceRequest>(inputGen);

                List<LineItem> items = new List<LineItem>();
            decimal taxTotal = 0; decimal price = 0; decimal total = 0; ; decimal discount = 0;
            int counter = 0; 
            var type = "00";
            var countryCode = "SA"; string invoiceType = string.Empty;
            InvoiceRequestDto request = new InvoiceRequestDto();
            List<InvoiceItemDto> invItems = new List<InvoiceItemDto>();
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
                
                if ( total < 1000 && countryCode == "SA")
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
                    ID = param.invoiceno.ToString(),
                    UUID = param.uniqueIdentifier.ToString(),
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
                        Value = GetInvoiceTypeCode(param.invoiceType)  //Invoice type specific
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
                                    UUID= param.invoiceno.ToString(),
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
                                            Value = null
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
                                            Value = input.Supplier[0].CRNumber
                                        }
                                    }
                                },

                            PostalAddress = new AddressType
                            {
                                StreetName = input.Supplier[0].Address.Street ?? "",
                                BuildingNumber = input.Supplier[0].Address.BuildingNo ?? "",
                                PlotIdentification = input.Supplier[0].Address.AdditionalNo ?? "",
                                CitySubdivisionName = input.Supplier[0].Address.AdditionalStreet ?? "",
                                CityName = input.Supplier[0].Address.City ?? "",
                                PostalZone = input.Supplier[0].Address.PostalCode ?? "",
                                CountrySubentity = input.Supplier[0].Address.State ?? "",
                                Country = new CountryType
                                {
                                    IdentificationCode = new CodeType
                                    {
                                        Value = input.Supplier[0].Address.CountryCode ?? ""
                                    }
                                }
                            },
                            PartyTaxScheme = new List<PartyTaxSchemeType>()
                                    {
                                        new PartyTaxSchemeType
                                        {
                                            CompanyID = new IdentifierType
                                            {
                                                  Value = input.Supplier[0].VATID
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
                                            RegistrationName = input.Supplier[0].RegistrationName
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
                            //            ElectronicMail = input.Buyer[0].ContactPerson.Email,
                            //            Telephone = input.Buyer[0].ContactPerson.ContactNumber,
                            //            Name = input.Buyer[0].ContactPerson.Name

                            //        }       //recheck
                            //        }
                            //    },

                            PostalAddress = new AddressType
                            {
                                StreetName = input.Buyer[0].Address.Street ?? "",
                                BuildingNumber = input.Buyer[0].Address.BuildingNo ?? "",
                                PlotIdentification = input.Buyer[0].Address.AdditionalNo ?? "",
                                CitySubdivisionName = input.Buyer[0].Address.AdditionalStreet ?? "",
                                CityName = input.Buyer[0].Address.City ?? "",
                                PostalZone = input.Buyer[0].Address.PostalCode ?? "",
                                CountrySubentity = input.Buyer[0].Address.State ?? "",
                                Country = new CountryType
                                {
                                    IdentificationCode = new CodeType
                                    {
                                        Value = input.Supplier[0].Address.CountryCode
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
                                                Value=input.Buyer[0].VATID
                                            }
                                        }
                                    },
                            PartyLegalEntity = new List<PartyLegalEntityType>()
                                    {
                                        new PartyLegalEntityType
                                        {
                                           RegistrationName = input.Buyer[0].RegistrationName
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
                                    },
                                    InstructionNote = new List<TextType>()
                                    {
                                        new TextType()
                                        {
                                            Value=input.InvoiceNotes
            }
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
                               Value = 0.00m
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
                    TaxTotal = UpdateTaxTotal(input.VATDetails),
                    LegalMonetaryTotal = new MonetaryTotalType
                    {
                        LineExtensionAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(1.00m * price, 2)
                        },
                        TaxExclusiveAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(1.00m * price, 2)
                        },
                        TaxInclusiveAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(1.00m * total, 2)
                        },
                        AllowanceTotalAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = 0.00m
                        },
                        PayableAmount = new AmountType
                        {
                            currencyID = "SAR",
                            Value = Math.Round(1.00m * total, 2)
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
                                Value = Math.Round(1.00m * (decimal)(item.Quantity * item.UnitPrice)- (decimal)item.DiscountAmount, 2)
                            },
                            TaxTotal = new List<TaxTotalType>()
                                {
                                        new TaxTotalType
                                        {
                                            TaxAmount = new AmountType
                                            {
                                                currencyID = "SAR",
                                                Value = Math.Round(1.00m * item.VATAmount,2)
                                            },
                                            RoundingAmount = new AmountType
                                            {
                                              currencyID= "SAR",
                                              Value=Math.Round(1.00m * item.LineAmountInclusiveVAT,2)
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
                                                    Value = item.VATCode
                                                },
                                                Percent = Math.Round(1.00m * item.VATRate,2),
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
                                    Value = Math.Round(1.00m * (decimal)item.UnitPrice- (decimal)item.DiscountAmount, 2)
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
                                            Value = Math.Round(1.00m * (decimal)item.DiscountAmount,2)
                                        },
                                        AllowanceChargeReason=new List<TextType>()
                                        {
                                            new TextType
                                            {
                                                Value="discount"
                                            }
                                        },
                                        BaseAmount = new AmountType
                                        {
                                            currencyID="SAR",
                                            Value = Math.Round(1.00m * item.UnitPrice,2)
                                        }
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
                if (param.tenantId != null && param.tenantId != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + param.tenantId, param.uniqueIdentifier);
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0", param.uniqueIdentifier);
                var xmlfileName = input.Supplier[0].VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + param.invoiceno + ".xml";
                var path = (Path.Combine(pathToSave, xmlfileName));

          
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(InvoiceType));
                var xml = "";

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, doc, doc.Xmlns);
                    xml= textWriter.ToString();
                }

                var log = await SignXml(new XMLSignInput()
                {
                    xml = xml,
                    UUID= param.uniqueIdentifier,
                    xml_uuid= param.xml_uid,
                    qrPath = Path.Combine(pathToSave, param.uniqueIdentifier +"_"+ param.invoiceno +".png"),
                    SellerName=input.Supplier[0].RegistrationName,
                    VatId = input.Supplier[0].VATID,
                    TotalAmount = input.InvoiceSummary.TotalAmountWithVAT,
                    VatAmount = input.InvoiceSummary.TotalVATAmount,
                    IssueDate = input.IssueDate,
                    xmlPath = path,
                    pathToSave = pathToSave,
                    xmlModel = doc,
                    irnno = Convert.ToInt32(param.invoiceno),
                    tenantId = Convert.ToInt32(param.tenantId),
                    isPhase1  = param.isPhase1
                });

                using (var uow = UnitOfWorkManager.Begin())
                {
                    LogXML(log);
                    uow.Complete();
                }

                return true;

            }

            catch (Exception ex)
            {
                return false;
            }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Error generating XML");
            }
        }

    }

}
