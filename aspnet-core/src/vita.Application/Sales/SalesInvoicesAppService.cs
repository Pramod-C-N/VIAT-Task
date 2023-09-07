using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Sales.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using vita.MasterData;
using vita.MasterData.Dtos;
using System.IO;
using vita.Utils;
using vita.UblSharp;
using vita.PdfFile;
using System.Data;
using Microsoft.Data.SqlClient;
using Abp.EntityFrameworkCore;
using vita.EntityFrameworkCore;
using NPOI.HPSF;
using NPOI.POIFS.Crypt.Dsig;
using vita.TenantDetails;
using vita.Report.Dto;
using Z.EntityFramework.Plus;
using Abp.Domain.Uow;
using System.Transactions;
using Abp.Timing.Timezone;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JsonFlatten;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Text;
using NPOI.HSSF.Util;

namespace vita.Sales
{
    [AbpAuthorize(AppPermissions.Pages_SalesInvoices)]
    public class SalesInvoicesAppService : vitaAppServiceBase, ISalesInvoicesAppService
    {
        private readonly IRepository<SalesInvoice, long> _salesInvoiceRepository;
        private readonly ISalesInvoicePartiesAppService _partyAppService;
        private readonly ISalesInvoiceItemsAppService _invoiceItemsAppService;
        private readonly ISalesInvoiceVATDetailsAppService _vatDetailsAppService;
        private readonly ISalesInvoicePaymentDetailsAppService _paymentDetailsAppService;
        private readonly ISalesInvoiceSummariesAppService _invoiceSummariesAppService;
        private readonly ISalesInvoiceDiscountsAppService _discountAppService;
        private readonly ISalesInvoiceContactPersonsAppService _contactPersonsAppService;
        private readonly ISalesInvoiceAddressesAppService _invoiceAddressesAppService;
        private readonly IIRNMastersAppService _transactionsAppService;
        private readonly IGenerateXmlAppService _generateXmlAppService;
        private readonly IPdfReportAppService _pdfReportAppService;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITenantBasicDetailsAppService _tenantbasicdetails;
        private readonly ITimeZoneConverter _timeZoneConverter;



        public SalesInvoicesAppService(IRepository<SalesInvoice, long> salesInvoiceRepository,
            ISalesInvoicePartiesAppService partyAppService,
       ISalesInvoiceItemsAppService invoiceItemsAppService,
       ISalesInvoiceVATDetailsAppService vatDetailsAppService,
       ISalesInvoicePaymentDetailsAppService paymentDetailsAppService,
       ISalesInvoiceSummariesAppService invoiceSummariesAppService,
       ISalesInvoiceDiscountsAppService discountAppService,
       ISalesInvoiceContactPersonsAppService contactPersonsAppService,
       ISalesInvoiceAddressesAppService invoiceAddressesAppService,
       IIRNMastersAppService transactionsAppService,
       IGenerateXmlAppService generateXmlAppService,
       IPdfReportAppService pdfReportAppService,
       ITenantBasicDetailsAppService tenantbasicdetails,
       IDbContextProvider<vitaDbContext> dbContextProvider,
       ITimeZoneConverter timeZoneConverter)
        {
            _salesInvoiceRepository = salesInvoiceRepository;
            _contactPersonsAppService = contactPersonsAppService;
            _discountAppService = discountAppService;
            _generateXmlAppService = generateXmlAppService;
            _invoiceAddressesAppService = invoiceAddressesAppService;
            _invoiceSummariesAppService = invoiceSummariesAppService;
            _invoiceItemsAppService = invoiceItemsAppService;
            _partyAppService = partyAppService;
            _vatDetailsAppService = vatDetailsAppService;
            _paymentDetailsAppService = paymentDetailsAppService;
            _transactionsAppService = transactionsAppService;
            _pdfReportAppService = pdfReportAppService;
            _dbContextProvider = dbContextProvider;
            _tenantbasicdetails = tenantbasicdetails;
            _timeZoneConverter = timeZoneConverter;

        }
        public async Task<int> GetLatestBatchId()
        {
            int batchId = 0;

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
                        cmd.CommandText = "GetLatestBatchId";

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            batchId = Convert.ToInt32(reader.GetValue(0));
                            break;
                        }
                        conn.Close();
                        return batchId;
                    }
                }

            }
            catch (Exception e)
            {
                return batchId;
            }
        }

        public async Task<string> ValidateRequest<T>(T data,int RuleGroupId)
        {
            var jsonString = JsonConvert.SerializeObject(data);

            var jObject = JObject.Parse(jsonString);
            var flattened = jObject.Flatten();

            var flattenedJsonString = JsonConvert.SerializeObject(flattened, Formatting.Indented); 

            string errors = null;
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
            public async Task<bool>InsertSalesReportData(long IRNNo)
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
                            cmd.CommandText = "InsertSalesReportData";

                            //cmd.Parameters.AddWithValue("@IRNNo", IRNNo);
                            cmd.Parameters.Add(new SqlParameter("IRNNo", SqlDbType.Int));
                            cmd.Parameters["IRNNo"].Value = IRNNo;
                            cmd.Parameters["IRNNo"].Direction = ParameterDirection.Input;

                            //cmd.Parameters.AddWithValue("@TenantId", AbpSession.TenantId);
                            cmd.Parameters.Add(new SqlParameter("TenantId", SqlDbType.Int));
                            cmd.Parameters["TenantId"].Value = AbpSession.TenantId;
                            cmd.Parameters["TenantId"].Direction = ParameterDirection.Input;


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
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }


        }


        public async Task<bool> UpdateInvoiceStatus(string status, int batchId, string refNo, int priority = 0, string irnno = null, string errors = "")
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
                        cmd.CommandText = "UpdateInvoiceStatus";

                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@batchId", batchId);
                        cmd.Parameters.AddWithValue("@refNo", refNo);
                        cmd.Parameters.AddWithValue("@priority", priority);
                        cmd.Parameters.AddWithValue("@irnno", irnno);
                        cmd.Parameters.AddWithValue("@errors", errors);

                        int i = cmd.ExecuteNonQuery();

                        conn.Close();

                        return i > 0;

                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public string[] uom = { "LTRS", "PCS", "NOS", "GMS", "KGS", "PACKS" };
        public string[] countryCode = { "SA" };
        public string[] taxCurrencyCode = { "SAR" };
        public string[] vatCode = { "S", "Z", "E", "O" };
        public async Task<string> Validator(CreateOrEditSalesInvoiceDto data, int batchId)
        {
            StringBuilder exceptionMessage = new StringBuilder();
            try
            {

                if (data.IssueDate > DateTime.Now)
                {
                    exceptionMessage.Append("Issue Date can't be greater than current date;");
                }
                if (data.InvoiceCurrencyCode.ToUpper() != "SAR")
                {
                    exceptionMessage.Append("Invoice Currency code should be SAR;");

                }
                if (!string.IsNullOrWhiteSpace(data.Buyer.VATID))
                {
                    if (!data.Buyer.VATID.StartsWith("3"))
                    {
                        exceptionMessage.Append("Buyer VAT Code should start with 3;");
                    }
                    if (data.Buyer.VATID.Trim().Length != 15)
                    {
                        exceptionMessage.Append("Buyer VAT Code should have 15 character length;");
                    }
                    if (!Regex.IsMatch(data.Buyer.VATID, "\\d{15}"))
                    {
                        exceptionMessage.Append("Buyer VAT Code should be Numeric;");
                    }

                }

                if (!uom.Contains(data.Items.FirstOrDefault()?.UOM?.ToUpper()))
                {
                    exceptionMessage.Append("Invalid UOM;");
                }
                if (!countryCode.Contains(data.Buyer?.Address?.CountryCode?.ToUpper()))
                {
                    exceptionMessage.Append("Invalid Buyer Country Code;");
                }
                if (!taxCurrencyCode.Contains(data.InvoiceCurrencyCode.ToUpper()))
                {
                    exceptionMessage.Append("Invalid Invoice Currency Code;");
                }
                int i = 1;
                foreach (var item in data.Items)
                {
                    if (item.GrossPrice <= 0)
                    {
                        exceptionMessage.Append("Item" + i + "-" + "Invalid Gross Price;");
                    }
                    if ((item.GrossPrice - item.DiscountAmount) != item.NetPrice)
                    {
                        exceptionMessage.Append("Item" + i + "-" + "Invalid Net Price;");
                    }

                    if (item.Quantity <= 0)
                    {
                        exceptionMessage.Append("Item" + i + "-" + "Invalid Quantity;");
                    }
                    if ((item.Quantity * (item.GrossPrice - item.DiscountAmount)) != item.LineAmountInclusiveVAT - item.VATAmount)
                    {
                        exceptionMessage.Append("Item" + i + "-" + "Invalid Line net amount;");
                    }
                    if (item.VATCode.ToUpper() == "E")
                    {
                        if (string.IsNullOrWhiteSpace(item.ExcemptionReasonCode))
                        {
                            exceptionMessage.Append("Item" + i + "-" + "Vat Exemption Reason Code is required;");
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(item.ExcemptionReasonCode))
                            {
                                exceptionMessage.Append("Item" + i + "-" + "Vat Exemption Reason is required;");

                            }
                        }
                    }
                    if (item.VATAmount != ((item.Quantity * (item.GrossPrice - (item.GrossPrice * item.DiscountAmount))) * (item.VATRate / 100)))
                    {
                        exceptionMessage.Append("Item" + i + "-" + "Invalid Vat Line Amount;");
                    }
                    if (item.LineAmountInclusiveVAT != (((item.Quantity * (item.GrossPrice - (item.GrossPrice * item.DiscountAmount))) * (item.VATRate / 100)) + (item.Quantity * (item.GrossPrice - (item.GrossPrice * (item.DiscountPercentage / 100))))))
                    {
                        exceptionMessage.Append("Item" + i + "-" + "Invalid Line Amount Inclusive VAT;");
                    }
                    i++;
                }

            }
            catch (System.Exception exception)
            {
                return exceptionMessage.ToString();

            }
            return exceptionMessage.ToString();

        }

        public async Task<bool> GenerateInvoice_SG(CreateOrEditSalesInvoiceDto input, int batchId)
        {
            TransactionDto data = new TransactionDto();
            string invoiceno = "";

            using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                await unitOfWork.CompleteAsync();
                input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;

                input.InvoiceCurrencyCode = "SAR";
                DataTable dt = new DataTable();
                dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


                if (input.Supplier == null)
                {
                    input.Supplier = new CreateOrEditSalesInvoicePartyDto();
                    input.Supplier.Address = new CreateOrEditSalesInvoiceAddressDto();

                }
                input.Supplier.ContactPerson = new CreateOrEditSalesInvoiceContactPersonDto();

                if (input.Buyer == null)
                {
                    input.Buyer = new CreateOrEditSalesInvoicePartyDto();
                    input.Buyer.Address = new CreateOrEditSalesInvoiceAddressDto();

                }
                input.Buyer.ContactPerson = new CreateOrEditSalesInvoiceContactPersonDto();


                var i = 0;
                foreach (DataRow row in dt.Rows)
                {

                    input.Supplier.VATID = row["vatid"].ToString();
                    //Address
                    input.Supplier.Address.AdditionalNo = row["AdditionalBuildingNumber"].ToString();
                    input.Supplier.Address.BuildingNo = row["BuildingNo"].ToString();
                    input.Supplier.Address.Street = row["Street"].ToString();
                    input.Supplier.Address.AdditionalStreet = row["AdditionalStreet"].ToString();
                    input.Supplier.Address.PostalCode = row["PostalCode"].ToString();
                    input.Supplier.Address.CountryCode = row["country"].ToString();
                    input.Supplier.Address.City = row["city"].ToString();
                    input.Supplier.Address.State = row["State"].ToString();
                    input.Supplier.CRNumber = row["DocumentNumber"].ToString();
                    input.Supplier.ContactPerson.ContactNumber = row["ContactNumber"].ToString();

                    i++;

                }
                input.Buyer.Address.Type = "Buyer";
                input.Buyer.ContactPerson.Type = "Buyer";
                input.Buyer.Type = "Buyer";
                input.Supplier.Address.Type = "Supplier";
                input.Supplier.ContactPerson.Type = "Supplier";
                input.Supplier.ContactPerson.Type = "Supplier";
                input.Supplier.Type = "Supplier";

                var a = new CreateOrEditIRNMasterDto()
                {
                    TransactionType = "SalesInvoice",
                };

                data = await _transactionsAppService.CreateOrEdit(a);
                invoiceno = data.IRNNo.ToString();
                input.IRNNo = invoiceno;
                input.Buyer.IRNNo = invoiceno;
                input.Supplier.IRNNo = invoiceno;
                input.Supplier.Address.IRNNo = invoiceno;
                input.Buyer.Address.IRNNo = invoiceno;
                input.Buyer.ContactPerson.IRNNo = invoiceno;
                input.Supplier.ContactPerson.IRNNo = invoiceno;
                input.InvoiceSummary.IRNNo = invoiceno;

                foreach (var item in input.Items)
                {
                    item.IRNNo = invoiceno;
                    if (string.IsNullOrWhiteSpace(item.VATCode))
                    {
                        if (item.VATRate == 15)
                            item.VATCode = "S";
                        else if (item.VATRate == 0)
                            item.VATCode = "Z";
                        else
                            item.VATCode = "S";
                    }
                }

                string errors = await Validator(input, batchId);

                if (errors != null && errors.Length > 0)
                    await UpdateInvoiceStatus("R", batchId, input.InvoiceNumber, 0, invoiceno, errors);
                else
                    await UpdateInvoiceStatus("V", batchId, input.InvoiceNumber, 1, invoiceno, errors);

                await CreateOrEdit(input);
                await _invoiceSummariesAppService.CreateOrEdit(input.InvoiceSummary);
                await _partyAppService.CreateOrEdit(input.Buyer);
                await _partyAppService.CreateOrEdit(input.Supplier);
                await _invoiceAddressesAppService.CreateOrEdit(input.Buyer.Address);
                await _invoiceAddressesAppService.CreateOrEdit(input.Supplier.Address);
                await _contactPersonsAppService.CreateOrEdit(input.Buyer.ContactPerson);
                await _contactPersonsAppService.CreateOrEdit(input.Supplier.ContactPerson);


                //--------------------newly added---------------------------
                if (input.PaymentDetails == null)
                {
                    var paymentD = new CreateOrEditSalesInvoicePaymentDetailDto()
                    {
                        PaymentMeans = "Cash",
                        PaymentTerms = "",
                        IRNNo = invoiceno

                    };
                    input.PaymentDetails = new List<CreateOrEditSalesInvoicePaymentDetailDto>();
                    input.PaymentDetails.Add(paymentD);
                    await _paymentDetailsAppService.CreateOrEdit(paymentD);

                }
                else
                {
                    foreach (var paymentDetail in input.PaymentDetails)
                    {
                        paymentDetail.IRNNo = invoiceno;
                        await _paymentDetailsAppService.CreateOrEdit(paymentDetail);
                    }
                }

                if (input.VATDetails == null)
                {
                    var details = input.Items.Select(p => p.VATCode).Distinct().ToList();
                    input.VATDetails = new List<CreateOrEditSalesInvoiceVATDetailDto>();

                    foreach (var item in details)
                    {
                        var vatdata = new CreateOrEditSalesInvoiceVATDetailDto();
                        vatdata.IRNNo = invoiceno;
                        vatdata.VATRate = (decimal)input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATRate;
                        vatdata.TaxSchemeId = "VAT";
                        vatdata.ExcemptionReasonCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonCode;
                        vatdata.ExcemptionReasonText = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonText;
                        vatdata.VATCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATCode;
                        vatdata.TaxAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.VATAmount);
                        vatdata.TaxableAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.NetPrice);
                        vatdata.CurrencyCode = "SAR";
                        input.VATDetails.Add(vatdata);
                        await _vatDetailsAppService.CreateOrEdit(vatdata);
                    }
                }
                else
                {
                    foreach (var vatDetail in input.VATDetails)
                    {
                        vatDetail.IRNNo = invoiceno;
                        await _vatDetailsAppService.CreateOrEdit(vatDetail);
                    }
                }

                //------------------------------------------------------

                foreach (var item in input.Items)
                {
                    await _invoiceItemsAppService.CreateOrEdit(item);
                }
            }


            // _dbContextProvider.GetDbContext().Database.CommitTransaction();

            var response = new InvoiceResponse();
            try
            {
                response.PdfFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
                response.XmlFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".xml";
                response.QRCodeUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".png";


                //_ = Task.Run(() =>
                //    XmlPdfJob(input, data.UniqueIdentifier, invoiceno, batchId)
                //) ;
                Guid uuid = data.UniqueIdentifier;
                await _generateXmlAppService.GenerateXmlRequest_Invoice(input, invoiceno, uuid.ToString(), AbpSession.TenantId.ToString(),input.Additional_Info);
                await UpdateInvoiceStatus("X", batchId, input.InvoiceNumber, 3);

                var pathToSave = string.Empty;
                if (AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + AbpSession.TenantId, uuid.ToString());
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", uuid.ToString());
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
                var path = (Path.Combine(pathToSave, xmlfileName));
                await _pdfReportAppService.GetPDFFile_Invoice(input, invoiceno, uuid.ToString(), AbpSession.TenantId.ToString());
                await UpdateInvoiceStatus("I", batchId, input.InvoiceNumber, 4);

                response.InvoiceNumber = invoiceno;
                response.QRCode = "";
                response.QRCodeUrl = "";
                response.ArchivalFileUrl = "";
                response.PreviousHash = "";
                response.TransactionCode = "";
                response.TypeCode = "";

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            //  await _dbContextProvider.GetDbContext().SaveChangesAsync();
            return true;

        }

        public async void XmlPdfJob(CreateOrEditSalesInvoiceDto input,Guid uuid,string invoiceno,int batchId)
        {
            try
            {

                await _generateXmlAppService.GenerateXmlRequest_Invoice(input, invoiceno, uuid.ToString(), AbpSession.TenantId.ToString());
                await UpdateInvoiceStatus("X", batchId, input.InvoiceNumber, 3);

                var pathToSave = string.Empty;
                if (AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + AbpSession.TenantId, uuid.ToString());
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", uuid.ToString());
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
                var path = (Path.Combine(pathToSave, xmlfileName));
                await _pdfReportAppService.GetPDFFile_Invoice(input, invoiceno, uuid.ToString(), AbpSession.TenantId.ToString());
                await UpdateInvoiceStatus("I", batchId, input.InvoiceNumber, 4);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        public async Task<bool> InsertBatchUploadSales(string json,string fileName,int? tenantId,DateTime? fromDate,DateTime? toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "InsertBatchUploadSales";
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);


                        await cmd.ExecuteNonQueryAsync();
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



        public async Task<DataTable> GetSalesBatchData(string fileName)
        {
            
            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr)) 
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetSalesBatchData";
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();

                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> getintegrationdashboraddata(DateTime fromDate, DateTime toDate, string type)
        {

            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getintegrationdashboraddata";
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();

                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }
        public async Task<DataTable> getintegrationdashboradcolor(DateTime fromDate, DateTime toDate, string type)
        {

            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getintegrationdashboradcolor";
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();

                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> GetStatsDashboardData(DateTime fromDate,DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

            DataTable dt = new DataTable(); 
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
                        cmd.CommandText = "GetStatsDashboardData";
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();

                        return dt;
                    }
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> GetSummaryDashboardData(DateTime fromDate, DateTime toDate,string type)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;

            DataTable dt = new DataTable();
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
                        cmd.CommandText = "GetSummaryDashboardData";
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@type", type);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();

                        return dt;
                    }
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> GetSalesInvalidRecord(int batchid)
        {


            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetSalesInvalidRecord";
                        cmd.Parameters.AddWithValue("@batchid", batchid);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> getInvoiceSuggestions(string irrno)
        {

            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getInvoiceSuggestions";
                        cmd.Parameters.AddWithValue("@irrno", irrno);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> getsalesdetails(int irrno)
        {

            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getsalesdetails";

                        cmd.Parameters.AddWithValue("@irrnNo", irrno);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> getsalesitemdetail(int irrno)
        {

            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr)) 
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getsalesitemdetail";

                        cmd.Parameters.AddWithValue("@irrnNo", irrno);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }





        public async Task<PagedResultDto<GetSalesInvoiceForViewDto>> GetAll(GetAllSalesInvoicesInput input)
        {

            var filteredSalesInvoices = _salesInvoiceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.InvoiceNumber.Contains(input.Filter) || e.InvoiceCurrencyCode.Contains(input.Filter) || e.CurrencyCodeOriginatingCountry.Contains(input.Filter) || e.PurchaseOrderId.Contains(input.Filter) || e.BillingReferenceId.Contains(input.Filter) || e.ContractId.Contains(input.Filter) || e.Location.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.Additional_Info.Contains(input.Filter) || e.PaymentType.Contains(input.Filter) || e.PdfUrl.Contains(input.Filter) || e.QrCodeUrl.Contains(input.Filter) || e.XMLUrl.Contains(input.Filter) || e.ArchivalUrl.Contains(input.Filter) || e.PreviousInvoiceHash.Contains(input.Filter) || e.PerviousXMLHash.Contains(input.Filter) || e.XMLHash.Contains(input.Filter) || e.PdfHash.Contains(input.Filter) || e.XMLbase64.Contains(input.Filter) || e.PdfBase64.Contains(input.Filter) || e.TransTypeDescription.Contains(input.Filter) || e.AdvanceReferenceNumber.Contains(input.Filter) || e.Invoicetransactioncode.Contains(input.Filter) || e.BusinessProcessType.Contains(input.Filter) || e.InvoiceNotes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceNumberFilter), e => e.InvoiceNumber.Contains(input.InvoiceNumberFilter))
                        .WhereIf(input.MinIssueDateFilter != null, e => e.IssueDate >= input.MinIssueDateFilter)
                        .WhereIf(input.MaxIssueDateFilter != null, e => e.IssueDate <= input.MaxIssueDateFilter)
                        .WhereIf(input.MinDateOfSupplyFilter != null, e => e.DateOfSupply >= input.MinDateOfSupplyFilter)
                        .WhereIf(input.MaxDateOfSupplyFilter != null, e => e.DateOfSupply <= input.MaxDateOfSupplyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceCurrencyCodeFilter), e => e.InvoiceCurrencyCode.Contains(input.InvoiceCurrencyCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeOriginatingCountryFilter), e => e.CurrencyCodeOriginatingCountry.Contains(input.CurrencyCodeOriginatingCountryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseOrderIdFilter), e => e.PurchaseOrderId.Contains(input.PurchaseOrderIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingReferenceIdFilter), e => e.BillingReferenceId.Contains(input.BillingReferenceIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContractIdFilter), e => e.ContractId.Contains(input.ContractIdFilter))
                        .WhereIf(input.MinLatestDeliveryDateFilter != null, e => e.LatestDeliveryDate >= input.MinLatestDeliveryDateFilter)
                        .WhereIf(input.MaxLatestDeliveryDateFilter != null, e => e.LatestDeliveryDate <= input.MaxLatestDeliveryDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.Contains(input.LocationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIdFilter), e => e.CustomerId.Contains(input.CustomerIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status.Contains(input.StatusFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTypeFilter), e => e.PaymentType.Contains(input.PaymentTypeFilter))
                        .WhereIf(input.IsArchivedFilter.HasValue && input.IsArchivedFilter > -1, e => (input.IsArchivedFilter == 1 && e.IsArchived) || (input.IsArchivedFilter == 0 && !e.IsArchived))
                        .WhereIf(input.MinTransTypeCodeFilter != null, e => e.TransTypeCode >= input.MinTransTypeCodeFilter)
                        .WhereIf(input.MaxTransTypeCodeFilter != null, e => e.TransTypeCode <= input.MaxTransTypeCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransTypeDescriptionFilter), e => e.TransTypeDescription.Contains(input.TransTypeDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdvanceReferenceNumberFilter), e => e.AdvanceReferenceNumber.Contains(input.AdvanceReferenceNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoicetransactioncodeFilter), e => e.Invoicetransactioncode.Contains(input.InvoicetransactioncodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessProcessTypeFilter), e => e.BusinessProcessType.Contains(input.BusinessProcessTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceNotesFilter), e => e.InvoiceNotes.Contains(input.InvoiceNotesFilter));

            var pagedAndFilteredSalesInvoices = filteredSalesInvoices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesInvoices = from o in pagedAndFilteredSalesInvoices
                                select new
                                {

                                    o.IRNNo,
                                    o.InvoiceNumber,
                                    o.IssueDate,
                                    o.DateOfSupply,
                                    o.InvoiceCurrencyCode,
                                    o.CurrencyCodeOriginatingCountry,
                                    o.PurchaseOrderId,
                                    o.BillingReferenceId,
                                    o.ContractId,
                                    o.LatestDeliveryDate,
                                    o.Location,
                                    o.CustomerId,
                                    o.Status,
                                    o.PaymentType,
                                    o.IsArchived,
                                    o.TransTypeCode,
                                    o.TransTypeDescription,
                                    o.AdvanceReferenceNumber,
                                    o.Invoicetransactioncode,
                                    o.BusinessProcessType,
                                    o.InvoiceNotes,
                                    Id = o.Id
                                };

            var totalCount = await filteredSalesInvoices.CountAsync();

            var dbList = await salesInvoices.ToListAsync();
            var results = new List<GetSalesInvoiceForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesInvoiceForViewDto()
                {
                    SalesInvoice = new SalesInvoiceDto
                    {

                        IRNNo = o.IRNNo,
                        InvoiceNumber = o.InvoiceNumber,
                        IssueDate = o.IssueDate,
                        DateOfSupply = o.DateOfSupply,
                        InvoiceCurrencyCode = o.InvoiceCurrencyCode,
                        CurrencyCodeOriginatingCountry = o.CurrencyCodeOriginatingCountry,
                        PurchaseOrderId = o.PurchaseOrderId,
                        BillingReferenceId = o.BillingReferenceId,
                        ContractId = o.ContractId,
                        LatestDeliveryDate = o.LatestDeliveryDate,
                        Location = o.Location,
                        CustomerId = o.CustomerId,
                        Status = o.Status,
                        PaymentType = o.PaymentType,
                        IsArchived = o.IsArchived,
                        TransTypeCode = o.TransTypeCode,
                        TransTypeDescription = o.TransTypeDescription,
                        AdvanceReferenceNumber = o.AdvanceReferenceNumber,
                        Invoicetransactioncode = o.Invoicetransactioncode,
                        BusinessProcessType = o.BusinessProcessType,
                        InvoiceNotes = o.InvoiceNotes,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSalesInvoiceForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalesInvoiceForViewDto> GetSalesInvoiceForView(long id)
        {
            var salesInvoice = await _salesInvoiceRepository.GetAsync(id);

            var output = new GetSalesInvoiceForViewDto { SalesInvoice = ObjectMapper.Map<SalesInvoiceDto>(salesInvoice) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoices_Edit)]
        public async Task<GetSalesInvoiceForEditOutput> GetSalesInvoiceForEdit(EntityDto<long> input)
        {
            var salesInvoice = await _salesInvoiceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesInvoiceForEditOutput { SalesInvoice = ObjectMapper.Map<CreateOrEditSalesInvoiceDto>(salesInvoice) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesInvoiceDto input)
        {
            await Create(input);
        }

        public async Task<DataTable> GetSalesData(DateTime fromDate, DateTime toDate)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
            DataTable dt = new DataTable(); try
            {
                var connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetSalesData";
                        cmd.Parameters.AddWithValue("fromDate", fromDate);
                        cmd.Parameters.AddWithValue("toDate", toDate);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        private async Task<InvoiceResponse> GenerateInvoice(CreateOrEditSalesInvoiceDto input)
        {
            input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;


           DataTable dt = new DataTable();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {

                input.Supplier.VATID = row["vatid"].ToString();
                //Address
                input.Supplier.Address.AdditionalNo = row["AdditionalBuildingNumber"].ToString();
                input.Supplier.Address.BuildingNo = row["BuildingNo"].ToString();
                input.Supplier.Address.Street = row["Street"].ToString();
                input.Supplier.Address.AdditionalStreet = row["AdditionalStreet"].ToString();
                input.Supplier.Address.PostalCode = row["PostalCode"].ToString();
                input.Supplier.Address.CountryCode = row["country"].ToString();
                input.Supplier.Address.City = row["city"].ToString();
                input.Supplier.Address.State = row["State"].ToString();
                input.Supplier.CRNumber = row["DocumentNumber"].ToString();
                input.Supplier.ContactPerson.ContactNumber = row["ContactNumber"].ToString();

                i++;

            }
            input.Buyer.Address.Type = "Buyer";
            input.Buyer.ContactPerson.Type = "Buyer";
            input.Buyer.Type = "Buyer";
            input.Supplier.Address.Type = "Supplier";
            input.Supplier.ContactPerson.Type = "Supplier";
            input.Supplier.ContactPerson.Type = "Supplier";
            input.Supplier.Type = "Supplier";

            var a = new CreateOrEditIRNMasterDto()
            {
                TransactionType = "SalesInvoice",
            };

            var data = await _transactionsAppService.CreateOrEdit(a);
            string invoiceno = data.IRNNo.ToString();
            input.IRNNo = invoiceno;
            input.Buyer.IRNNo = invoiceno;
            input.Supplier.IRNNo = invoiceno;
            input.Supplier.Address.IRNNo = invoiceno;
            input.Buyer.Address.IRNNo = invoiceno;
            input.Buyer.ContactPerson.IRNNo = invoiceno;
            input.Supplier.ContactPerson.IRNNo = invoiceno;
            input.InvoiceSummary.IRNNo = invoiceno;

            await CreateOrEdit(input);
            await _invoiceSummariesAppService.CreateOrEdit(input.InvoiceSummary);
            await _partyAppService.CreateOrEdit(input.Buyer);
            await _partyAppService.CreateOrEdit(input.Supplier);
            await _invoiceAddressesAppService.CreateOrEdit(input.Buyer.Address);
            await _invoiceAddressesAppService.CreateOrEdit(input.Supplier.Address);
            await _contactPersonsAppService.CreateOrEdit(input.Buyer.ContactPerson);
            await _contactPersonsAppService.CreateOrEdit(input.Supplier.ContactPerson);


            //--------------------newly added---------------------------
            if (input.PaymentDetails == null)
            {
                var paymentD = new CreateOrEditSalesInvoicePaymentDetailDto()
                {
                    PaymentMeans = "Cash",
                    PaymentTerms = "",
                    IRNNo = invoiceno

                };
                input.PaymentDetails = new List<CreateOrEditSalesInvoicePaymentDetailDto>();
                input.PaymentDetails.Add(paymentD);
                await _paymentDetailsAppService.CreateOrEdit(paymentD);

            }
            else
            {
                foreach (var paymentDetail in input.PaymentDetails)
                {
                    paymentDetail.IRNNo = invoiceno;
                    await _paymentDetailsAppService.CreateOrEdit(paymentDetail);
                }
            }

            if (input.VATDetails == null)
            {
                var details = input.Items.Select(p => p.VATCode).Distinct().ToList();
                input.VATDetails = new List<CreateOrEditSalesInvoiceVATDetailDto>();

                foreach (var item in details)
                {
                    var vatdata = new CreateOrEditSalesInvoiceVATDetailDto();
                    vatdata.IRNNo = invoiceno;
                    vatdata.VATRate = (decimal)input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATRate;
                    vatdata.TaxSchemeId = "VAT";
                    vatdata.ExcemptionReasonCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonCode;
                    vatdata.ExcemptionReasonText = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonText;
                    vatdata.VATCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATCode;
                    vatdata.TaxAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.VATAmount);
                    vatdata.TaxableAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.NetPrice);
                    vatdata.CurrencyCode = "SAR";
                    input.VATDetails.Add(vatdata);
                    await _vatDetailsAppService.CreateOrEdit(vatdata);
                }
            }
            else
            {
                foreach (var vatDetail in input.VATDetails)
                {
                    vatDetail.IRNNo = invoiceno;
                    await _vatDetailsAppService.CreateOrEdit(vatDetail);
                }
            }

            //------------------------------------------------------

            foreach (var item in input.Items)
            {
                item.IRNNo = invoiceno;
                if (string.IsNullOrWhiteSpace(item.VATCode))
                {
                    if (item.VATRate == 15)
                        item.VATCode = "S";
                    else if (item.VATRate == 0)
                        item.VATCode = "Z";
                    else
                        item.VATCode = "S";
                }
                await _invoiceItemsAppService.CreateOrEdit(item);
            }


            var response = new InvoiceResponse();
            try
            {
                response.PdfFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
                response.XmlFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".xml";
                response.QRCodeUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".png";

               await _generateXmlAppService.GenerateXmlRequest_Invoice(input, invoiceno, data.UniqueIdentifier.ToString(), AbpSession.TenantId.ToString(),data.UniqueIdentifier.ToString());

                //var newURL = Request.Scheme + "://" + Request.Host.Value + "/";

                var pathToSave = string.Empty;
                if (AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + AbpSession.TenantId, data.UniqueIdentifier.ToString());
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", data.UniqueIdentifier.ToString());
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);
                var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
                var path = (Path.Combine(pathToSave, xmlfileName));

                var xmlBase64 = FileIO.GetFileInBas64(path);
                var pdfBase64 = xmlBase64;
                var xmlHash = FileIO.GetSha256FileHash(path);
                var pdfHash = xmlHash;


                var isGenFile = await _pdfReportAppService.GetPDFFile_Invoice(input, invoiceno, data.UniqueIdentifier.ToString(), AbpSession.TenantId.ToString());


                response.InvoiceNumber = invoiceno;
                response.QRCode = "";
                response.QRCodeUrl = "";
                response.ArchivalFileUrl = "";
                response.PreviousHash = "";
                response.TransactionCode = "";
                response.TypeCode = "";

            }
            catch (Exception ex)
            {
                return new InvoiceResponse();
            }
            return new InvoiceResponse()
            {
                InvoiceId = Convert.ToInt32(invoiceno),
                InvoiceNumber = input.InvoiceNumber,
                Uuid = data.UniqueIdentifier,
            };
        }

        public async Task<InvoiceResponse> CreateSalesInvoice(CreateOrEditSalesInvoiceDto input)
        {
            var errors =await ValidateRequest(input, 2);
            if (errors != null)
            {
                throw new UserFriendlyException(errors.Replace(';','\n'));
            }
            InvoiceResponse data = new InvoiceResponse();

            // await _dbContextProvider.GetDbContext().SaveChangesAsync();
            // await _dbContextProvider.GetDbContext().Database.CommitTransactionAsync();
            //await _dbContextProvider.GetDbContext().Database.CloseConnectionAsync();
            using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                 data = await GenerateInvoice(input);
                await unitOfWork.CompleteAsync();
            }

            //   await CurrentUnitOfWork.SaveChangesAsync();
            await InsertSalesReportData(data.InvoiceId);


                return data;

        }
        
        [AbpAuthorize(AppPermissions.Pages_SalesInvoices_Create)]
        protected virtual async Task Create(CreateOrEditSalesInvoiceDto input)
        {
            {
                var salesInvoice = ObjectMapper.Map<SalesInvoice>(input);
                salesInvoice.UniqueIdentifier = Guid.NewGuid();
                if (AbpSession.TenantId != null)
                {
                    salesInvoice.TenantId = (int?)AbpSession.TenantId;
                }

                await _salesInvoiceRepository.InsertAsync(salesInvoice);

            }
        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoices_Edit)]
        protected virtual async Task Update(CreateOrEditSalesInvoiceDto input)
        {
            var salesInvoice = await _salesInvoiceRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, salesInvoice);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesInvoices_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _salesInvoiceRepository.DeleteAsync(input.Id);
        }

    }
}