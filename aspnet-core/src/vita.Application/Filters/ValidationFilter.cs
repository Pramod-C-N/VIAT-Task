using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Runtime.Session;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;
using System;
using vita.EInvoicing.Dto;
using vita.EntityFrameworkCore;
using vita.Filters;
using JsonFlatten;
using UblSharp;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Abp.Timing.Timezone;
using AutoMapper;
using vita.Sales.Dtos;
using vita.Credit.Dtos;
using vita.Debit.Dtos;

namespace vita.Filters
{
    public class ValidationFilter
    {
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IMapper mapper;

        public IAbpSession AbpSession { get; set; }

        public ValidationFilter(IDbContextProvider<vitaDbContext> dbContextProvider, IUnitOfWorkManager unitOfWorkManager, IAbpSession abpSession, ITimeZoneConverter timeZoneConverter,IMapper mapper)
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
            AbpSession = abpSession;
            _timeZoneConverter = timeZoneConverter;
            this.mapper = mapper;

        }
        private async Task<string> ValidateRequest<T>(T data, int RuleGroupId, string uuid = null)
        {

            var jsonString = JsonConvert.SerializeObject(data);

            var jObject = JObject.Parse(jsonString);
            if (uuid != null)
            {
                jObject.Add("uuid", uuid);
            }
            if (RuleGroupId == 3)
            {
                jObject.Remove("Id");
                jObject.Remove("IRNNo");

            }
            var flattened = jObject.Flatten();

            var flattenedJsonString = JsonConvert.SerializeObject(flattened, Formatting.Indented);

            string errors = null;
            SqlConnection conn = null;
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {

                    var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                    using (conn = new SqlConnection(connStr))
                    {

                        using (SqlCommand cmd = new SqlCommand())
                        {

                            conn.Open();
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "ExecuteRulesAPI";


                            cmd.Parameters.AddWithValue("@RuleGroupId", RuleGroupId);
                            cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                            cmd.Parameters.AddWithValue("@json", flattenedJsonString);


                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    errors = reader.GetString(0);
                                break;
                            }
                            conn.Close();
                            return errors;
                        }
                    }
                    await unitOfWork.CompleteAsync();
                }

                return errors;
            }
            catch (Exception e)
            {

                return e.Message.ToString();

            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }
        }

        public async Task<(string, List<VitaValidationDto>)> SalesAPIValidationAsync(string body)
        {
            try
            {
                var input = new CreateOrEditSalesInvoiceDto();
                input = JsonConvert.DeserializeObject<CreateOrEditSalesInvoiceDto>(body);


                var uuid = Guid.NewGuid().ToString();
                var errors = "";
                var e = "";
                foreach (var i in input.Items)
                {
                    e = await ValidateRequest(i, 3, uuid) ?? "";
                    errors += e;
                }
                e = await ValidateRequest(input, 2, uuid) ?? "";
                errors += e;
                e = await ValidateRequest(input, 4, uuid) ?? "";
                errors += e;

                return FormatErrorString(errors);

            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }

        public async Task<(string, List<VitaValidationDto>)> CreditAPIValidationAsync(string body)
        {
            try
            {
                var input = new CreateOrEditCreditNoteDto();
                input = JsonConvert.DeserializeObject<CreateOrEditCreditNoteDto>(body);


                var uuid = Guid.NewGuid().ToString();
                var errors = "";
                var e = "";
                foreach (var i in input.Items)
                {
                    e = await ValidateRequest(i, 3, uuid) ?? "";
                    errors += e;
                }
                e = await ValidateRequest(input, 2, uuid) ?? "";
                errors += e;
                e = await ValidateRequest(input, 4, uuid) ?? "";
                errors += e;

                return FormatErrorString(errors);

            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }


        public async Task<(string, List<VitaValidationDto>)> DebitAPIValidationAsync(string body)
        {
            try
            {
                var input = new CreateOrEditDebitNoteDto();
                input = JsonConvert.DeserializeObject<CreateOrEditDebitNoteDto>(body);


                var uuid = Guid.NewGuid().ToString();
                var errors = "";
                var e = "";
                foreach (var i in input.Items)
                {
                    e = await ValidateRequest(i, 3, uuid) ?? "";
                    errors += e;
                }
                e = await ValidateRequest(input, 2, uuid) ?? "";
                errors += e;
                e = await ValidateRequest(input, 4, uuid) ?? "";
                errors += e;

                return FormatErrorString(errors);

            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }


        public async Task<(string, List<VitaValidationDto>)> EInvoiceAPIValidationAsync(string body)
        {
            try
            {
                var input = new InvoiceRequest();
                input = JsonConvert.DeserializeObject<InvoiceRequest>(body);


                var uuid = Guid.NewGuid().ToString();
                var errors = "";
                var e = "";
                foreach (var i in input.Items)
                {
                    e = await ValidateRequest(i, 3, uuid) ?? "";
                    errors += e;
                }
                e = await ValidateRequest(input, 2, uuid) ?? "";
                errors += e;
                e = await ValidateRequest(input, 4, uuid) ?? "";
                errors += e;

                return FormatErrorString(errors);

            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }

        public async Task<(string, List<VitaValidationDto>)> UnicoreAPIValidationAsync(string body)
        {
            try
            {
                var input = new InvoiceRequest();

                var temp = JsonConvert.DeserializeObject<InvoiceRequestLanguage>(body);

                input = mapper.Map<InvoiceRequest>(temp);




                var uuid = Guid.NewGuid().ToString();
                var errors = "";
                var e = "";
                foreach (var i in input.Items)
                {
                    e = await ValidateRequest(i, 3, uuid) ?? "";
                    errors += e;
                }
                e = await ValidateRequest(input, 2, uuid) ?? "";
                errors += e;
                e = await ValidateRequest(input, 4, uuid) ?? "";
                errors += e;

                return FormatErrorString(errors);

            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }


        public async Task<(string, List<VitaValidationDto>, string)> EInvoiceAPIValidationXMLAsync(string body)
        {
            try
            {
                var input = new InvoiceRequest();


                var xmlReq = JsonConvert.DeserializeObject<InvoiceXMLRequest>(body);
                var deserializedXML = DeserializeXML(xmlReq.xmlString);
                input = InvoiceTypeToInvoiceDto(deserializedXML);
                xmlReq.InvoiceRequest = input;
                xmlReq.isParsed = true;


                var uuid = Guid.NewGuid().ToString();
                var errors = "";
                var e = "";

                foreach (var i in input.Items)
                {
                    e = await ValidateRequest(i, 3, uuid) ?? "";
                    errors += e;
                }
                e = await ValidateRequest(input, 2, uuid) ?? "";
                errors += e;
                e = await ValidateRequest(input, 4, uuid) ?? "";
                errors += e;
                var message = "";
                var errorList = new List<VitaValidationDto>();
                if (input.InvoiceType == null)
                {
                    errors = "UN-01--Invoice Type Code is missing";
                }
                (message, errorList) = FormatErrorString(errors);
                return (message, errorList, JsonConvert.SerializeObject(xmlReq));
            }
            catch (Exception e)
            {
                return (e.Message, null, null);
            }
        }

        private (string, List<VitaValidationDto>) FormatErrorString(string errors)
        {
            try
            {
                if (errors != null && errors != "")
                {
                    errors = errors.Remove(errors.Length - 1);
                    var errorArr = errors.Replace("\r", "").Replace("\n", "").Split(';');
                    var query = from s in errorArr
                                let x = s.Split("--")
                                select new VitaValidationDto { ErrorCode = x[0], ErrorMessage = x[1] };
                    return ("Validation error occured", query.ToList());

                }
                else
                {
                    return (null, null);
                }
            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }

        private InvoiceType DeserializeXML(string xml)
        {
            var result = new InvoiceType();

            XmlSerializer serializer = new XmlSerializer(typeof(InvoiceType));

            using (TextReader reader = new StringReader(xml))
            {
                result = (InvoiceType)serializer.Deserialize(reader);
            }

            return result;

        }

        private InvoiceTypeEnum? GetInvoiceType(string code)
        {
            if (code == "388")
            {
                return InvoiceTypeEnum.Sales;
            }
            else if (code == "381")
            {
                return InvoiceTypeEnum.Credit;
            }
            else if (code == "383")
            {
                return InvoiceTypeEnum.Debit;
            }
            else return null;
        }

        private InvoiceRequest InvoiceTypeToInvoiceDto(InvoiceType input)
        {
            InvoiceRequest result = new InvoiceRequest();

            result.InvoiceType = GetInvoiceType(input?.InvoiceTypeCode.Value);
            result.Supplier = new();
            result.Supplier.Add(new());
            result.Supplier[0].ContactPerson = new InvoiceContactPersonDto();
            result.Supplier[0].Address = new InvoiceAddressDto();
            result.Buyer = new();
            result.Buyer.Add(new());
            result.Buyer[0].ContactPerson = new InvoiceContactPersonDto();
            result.Buyer[0].Address = new InvoiceAddressDto();

            result.InvoiceSummary = new InvoiceSummaryDto();
            result.Items = new List<InvoiceItemDto>();

            result.Supplier[0].CRNumber = input?.AccountingSupplierParty?.Party?.PartyIdentification?.FirstOrDefault(a => a.ID.schemeID == "CRN")?.ID?.Value;
            result.Supplier[0].VATID = input?.AccountingSupplierParty?.Party?.PartyTaxScheme?.FirstOrDefault()?.CompanyID?.Value;
            result.Supplier[0].RegistrationName = input?.AccountingSupplierParty?.Party?.PartyLegalEntity?.FirstOrDefault()?.RegistrationName?.Value;

            result.Supplier[0].Address.Street = input?.AccountingSupplierParty?.Party?.PostalAddress?.StreetName;
            result.Supplier[0].Address.BuildingNo = input?.AccountingSupplierParty?.Party?.PostalAddress?.BuildingNumber;
            result.Supplier[0].Address.AdditionalNo = input?.AccountingSupplierParty?.Party?.PostalAddress?.PlotIdentification;
            result.Supplier[0].Address.AdditionalStreet = input?.AccountingSupplierParty?.Party?.PostalAddress?.CitySubdivisionName;
            result.Supplier[0].Address.City = input?.AccountingSupplierParty?.Party?.PostalAddress?.CityName;
            result.Supplier[0].Address.PostalCode = input?.AccountingSupplierParty?.Party?.PostalAddress?.PostalZone;
            result.Supplier[0].Address.State = input?.AccountingSupplierParty?.Party?.PostalAddress?.CountrySubentity;
            result.Supplier[0].Address.CountryCode = input?.AccountingSupplierParty?.Party?.PostalAddress?.Country.IdentificationCode;
            result.Buyer[0].Address.Neighbourhood = input?.AccountingSupplierParty?.Party?.PostalAddress?.Country.IdentificationCode;

            result.Supplier[0].Address.Type = "Supplier";
            result.Supplier[0].Type = "Supplier";
            result.Buyer[0].CRNumber = input?.AccountingCustomerParty?.Party?.PartyIdentification?.FirstOrDefault(a => a.ID.schemeID == "CRN")?.ID?.Value;
            result.Buyer[0].VATID = input?.AccountingCustomerParty?.Party?.PartyTaxScheme?.FirstOrDefault()?.CompanyID?.Value;
            result.Buyer[0].RegistrationName = input?.AccountingCustomerParty?.Party?.PartyLegalEntity?.FirstOrDefault()?.RegistrationName?.Value;

            result.Buyer[0].Address.Street = input?.AccountingCustomerParty?.Party?.PostalAddress?.StreetName;
            result.Buyer[0].Address.BuildingNo = input?.AccountingCustomerParty?.Party?.PostalAddress?.BuildingNumber;
            result.Buyer[0].Address.AdditionalNo = input?.AccountingCustomerParty?.Party?.PostalAddress?.PlotIdentification;
            result.Buyer[0].Address.AdditionalStreet = input?.AccountingCustomerParty?.Party?.PostalAddress?.CitySubdivisionName;
            result.Buyer[0].Address.City = input?.AccountingCustomerParty?.Party?.PostalAddress?.CityName;
            result.Buyer[0].Address.PostalCode = input?.AccountingCustomerParty?.Party?.PostalAddress?.PostalZone;
            result.Buyer[0].Address.State = input?.AccountingCustomerParty?.Party?.PostalAddress?.CountrySubentity;
            result.Buyer[0].Address.CountryCode = input?.AccountingCustomerParty?.Party?.PostalAddress?.Country.IdentificationCode;
            result.Buyer[0].Address.Neighbourhood = input?.AccountingCustomerParty?.Party?.PostalAddress?.Country.IdentificationCode;

            result.Buyer[0].Address.Type = "Buyer";
            result.Buyer[0].Type = "Buyer";
            result.Buyer[0].ContactPerson.Email = input?.AccountingSupplierParty?.Party?.Contact?.ElectronicMail;
            result.Buyer[0].ContactPerson.Type = "Buyer";

            result.InvoiceNumber = input?.ID;
            try
            {
                result.IssueDate= _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;
                // result.IssueDate = Convert.ToDateTime(input?.IssueDate);
            }
            catch (Exception ex)
            {
                result.IssueDate = DateTime.MinValue;
            }
            result.DateOfSupply = result?.IssueDate;
            result.InvoiceCurrencyCode = "SAR";
            result.Status = "Paid";
            result.PaymentType = "Cash";
            result.InvoiceNotes = input?.PaymentMeans?.FirstOrDefault()?.InstructionNote?.FirstOrDefault()?.Value;



            foreach (var item in input?.InvoiceLine)
            {
                var i = new InvoiceItemDto();

                i.Identifier = item?.ID ?? "Sales";
                i.Name = item?.Item?.Name;
                i.Description = item?.Item?.Description.FirstOrDefault()?.Value ?? "";
                i.Quantity = (decimal)(item?.InvoicedQuantity?.Value);
                i.UOM = item?.InvoicedQuantity?.unitCode;
                i.UnitPrice = (decimal)item?.Price?.PriceAmount?.Value;
                i.DiscountAmount = (decimal)(item?.Price?.AllowanceCharge?.FirstOrDefault()?.Amount?.Value ?? 0);
                i.DiscountPercentage = 0; //to be checked;
                i.NetPrice = (decimal)item?.Price?.PriceAmount?.Value * (decimal)(item?.InvoicedQuantity?.Value) - (decimal)(item?.Price?.AllowanceCharge?.FirstOrDefault()?.Amount?.Value ?? 0);
                i.VATAmount = (decimal)item?.TaxTotal?.FirstOrDefault()?.TaxAmount?.Value;
                i.VATCode = item?.Item?.ClassifiedTaxCategory?.FirstOrDefault()?.ID?.Value;
                i.VATRate = item?.Item?.ClassifiedTaxCategory?.FirstOrDefault()?.Percent;
                i.LineAmountInclusiveVAT = (decimal)item?.TaxTotal?.FirstOrDefault()?.RoundingAmount?.Value;
                i.CurrencyCode = "SAR";

                result.Items.Add(i);
            }

            result.InvoiceSummary.NetInvoiceAmount = (decimal)(result?.Items?.Sum(a => a.UnitPrice * a.Quantity));
            result.InvoiceSummary.NetInvoiceAmountCurrency = "SAR";
            result.InvoiceSummary.SumOfInvoiceLineNetAmount = (decimal)(result?.Items?.Sum(a => a.NetPrice));
            result.InvoiceSummary.TotalAmountWithoutVAT = (decimal)(result?.Items?.Sum(a => a.LineAmountInclusiveVAT)) - (decimal)(result?.Items?.Sum(a => a.VATAmount));
            result.InvoiceSummary.TotalAmountWithoutVATCurrency = "SAR";
            result.InvoiceSummary.TotalVATAmount = (decimal)(result?.Items?.Sum(a => a.VATAmount));
            result.InvoiceSummary.CurrencyCode = "SAR";
            result.InvoiceSummary.TotalAmountWithVAT = (decimal)(result?.Items?.Sum(a => a.LineAmountInclusiveVAT));
            result.InvoiceSummary.PaidAmountCurrency = "SAR";
            result.InvoiceSummary.PayableAmountCurrency = "SAR";

            




            return result;


        }
    }

}
