using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Sales.Dtos;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
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
using vita.TenantDetails;
using Z.EntityFramework.Plus;
using System.Transactions;
using Abp.Timing.Timezone;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using vita.Filters;
using static vita.Filters.VitaFilter_Validation;
using DbDataReaderMapper;
using System.Linq.Expressions;
using System.DirectoryServices.Protocols;
using AutoMapper;
using vita.EInvoicing.Dto;
using vita.MultiTenancy.Accounting.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Data.Common;
using vita.TenantConfigurations;
using vita.TenantConfigurations.Dtos;
using static vita.PdfFile.PdfReportAppService;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.Mappers;
using NPOI.SS.Formula.Functions;
using IdentityServer4.Models;
using NPOI.POIFS.Crypt.Dsig;
using vita.Credit;
using vita.Debit;
using Abp.UI;

namespace vita.Sales
{

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
      

        private readonly IMapper mapper;
        private readonly ITenantConfigurationAppService tenantConfigurationAppService;
        private string connStr = "";

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
       ITimeZoneConverter timeZoneConverter,
       IMapper mapper,
       ITenantConfigurationAppService tenantConfigurationAppService)
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
            this.mapper = mapper;
            this.tenantConfigurationAppService = tenantConfigurationAppService;
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
                        cmd.Parameters.Add(new SqlParameter("tenantId", SqlDbType.Int));
                        cmd.Parameters["tenantId"].Value = AbpSession.TenantId;
                        cmd.Parameters["tenantId"].Direction = ParameterDirection.Input;

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            batchId = Convert.ToInt32(reader["BatchId"]);
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

        public async Task<List<GetPdfIrnData>> GetIrnForFileUpload(int batchid)
        {
            List<GetPdfIrnData> irnNos = new();
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
                        cmd.CommandText = "GetIrnForFileUpload";
                        cmd.Parameters.Add(new SqlParameter("batchid", SqlDbType.Int));
                        cmd.Parameters["batchid"].Value = batchid;
                        cmd.Parameters["batchid"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(new SqlParameter("tenantId", SqlDbType.Int));
                        cmd.Parameters["tenantId"].Value = AbpSession.TenantId;
                        cmd.Parameters["tenantId"].Direction = ParameterDirection.Input;
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var data = reader.MapToObject<GetPdfIrnData>();
                            irnNos.Add(data);
                        }
                        conn.Close();
                        return irnNos;
                    }
                }

            }
            catch (Exception e)
            {
                return irnNos;
            }
        }


        public async Task<bool> InsertSalesReportData(long IRNNo)
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

        public class InvoiceStatusType
        {
            public int? TenantId { get; set; }
            public string status { get; set; }
            public int batchId { get; set; }
            public string refNo { get; set; }
            public string invoiceType { get; set; }
            public string irnno { get; set; }
            public string inputData { get; set; }
            public string errors { get; set; } = "";
            public bool isXmlSigned { get; set; } = false;
            public bool isPdfGenerated { get; set; } = false;
        }


        public async Task<InvoiceStatusType> GetInvoiceStatus(string TenantId, string irnno)
        {
            InvoiceStatusType invoiceStatusType = null;
            try
            {
                string connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetInvoiceStatus";
                        cmd.Parameters.AddWithValue("@irnno", irnno);
                        cmd.Parameters.AddWithValue("@TenantId", TenantId);

                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            invoiceStatusType = new InvoiceStatusType()
                            {
                                inputData = reader.GetString(0),
                                irnno = irnno.ToString(),
                                TenantId = Convert.ToInt32(TenantId)
                            };
                            break;
                        }

                        conn.Close();


                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return invoiceStatusType;
        }


        public async Task<bool> UpdateInvoiceStatus(InvoiceStatusType invStatus)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "UpdateInvoiceStatus";

                        cmd.Parameters.AddWithValue("@status", invStatus.status);
                        cmd.Parameters.AddWithValue("@batchId", invStatus.batchId);
                        cmd.Parameters.AddWithValue("@refNo", invStatus.refNo);
                        cmd.Parameters.AddWithValue("@irnno", invStatus.irnno);
                        cmd.Parameters.AddWithValue("@errors", invStatus.errors);
                        cmd.Parameters.AddWithValue("@invoiceType", invStatus.invoiceType);
                        cmd.Parameters.AddWithValue("@isXmlSigned", invStatus.isXmlSigned);
                        cmd.Parameters.AddWithValue("@isPdfGenerated", invStatus.isPdfGenerated);
                        cmd.Parameters.AddWithValue("@inputData", invStatus.inputData);
                        cmd.Parameters.AddWithValue("@TenantId", invStatus.TenantId);

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

        public async Task<bool> UpdateInvoiceURL(InvoiceResponse response, string type)
        {

            try
            {
                var connStr1 = _dbContextProvider.GetDbContext().Database.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr1))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "UpdateInvoiceURL";

                        cmd.Parameters.AddWithValue("@pdfurl", response.PdfFileUrl);
                        cmd.Parameters.AddWithValue("@xmlurl", response.PdfFileUrl);
                        cmd.Parameters.AddWithValue("@qrurl", response.PdfFileUrl);
                        cmd.Parameters.AddWithValue("@irn", response.InvoiceId.ToString());
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);


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


                if (!uom.Contains(data.Items.FirstOrDefault()?.UOM?.ToUpper()))
                {
                    exceptionMessage.Append("Invalid UOM;");
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

        //public async Task<InvoiceResponse> GenerateInvoice_SG(CreateOrEditSalesInvoiceDto input, int batchId)
        //{
        //    TransactionDto data = new TransactionDto();
        //    string invoiceno = "";
        //    bool isPhase1 = true;

        //    using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
        //    {
        //        input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;

        //        input.InvoiceCurrencyCode = "SAR";
        //        DataTable dt = new DataTable();
        //        dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


        //        if (input.Supplier == null)
        //        {
        //            input.Supplier = new List<CreateOrEditSalesInvoicePartyDto>();
        //            input.Supplier[0].Address = new CreateOrEditSalesInvoiceAddressDto();
        //        }
        //        input.Supplier[0].ContactPerson = new CreateOrEditSalesInvoiceContactPersonDto();

        //        if (input.Buyer == null)
        //        {
        //            input.Buyer = new List<CreateOrEditSalesInvoicePartyDto>();
        //        }


        //        var i = 0;
        //        foreach (DataRow row in dt.Rows)
        //        {

        //            input.Supplier[0].VATID = row["vatid"].ToString();
        //            //Address
        //            input.Supplier[0].Address.AdditionalNo = row["AdditionalBuildingNumber"].ToString();
        //            input.Supplier[0].Address.BuildingNo = row["BuildingNo"].ToString();
        //            input.Supplier[0].Address.Street = row["Street"].ToString();
        //            input.Supplier[0].Address.AdditionalStreet = row["AdditionalStreet"].ToString();
        //            input.Supplier[0].Address.PostalCode = row["PostalCode"].ToString();
        //            input.Supplier[0].Address.CountryCode = row["country"].ToString();
        //            input.Supplier[0].Address.City = row["city"].ToString();
        //            input.Supplier[0].Address.State = row["State"].ToString();
        //            input.Supplier[0].CRNumber = row["DocumentNumber"].ToString();
        //            input.Supplier[0].ContactPerson.ContactNumber = row["ContactNumber"].ToString();
        //            isPhase1 = Convert.ToInt32(row["isPhase1"] ?? 0) == 1;

        //            i++;

        //        }

        //        input.Supplier[0].Address.Type = "Supplier";
        //        input.Supplier[0].ContactPerson.Type = "Supplier";
        //        input.Supplier[0].ContactPerson.Type = "Supplier";
        //        input.Supplier[0].Type = "Supplier";

        //        var a = new CreateOrEditIRNMasterDto()
        //        {
        //            TransactionType = "SalesInvoice",
        //        };

        //        data = await _transactionsAppService.CreateOrEdit(a);
        //        invoiceno = data.IRNNo.ToString();
        //        input.IRNNo = invoiceno;
        //        input.Supplier[0].IRNNo = invoiceno;
        //        input.Supplier[0].Address.IRNNo = invoiceno;
        //        input.Supplier[0].ContactPerson.IRNNo = invoiceno;
        //        input.InvoiceSummary.IRNNo = invoiceno;

        //        foreach(var buyer in input.Buyer)
        //        {
        //            buyer.IRNNo = invoiceno;
        //            buyer.Address.IRNNo = invoiceno;
        //            buyer.ContactPerson.IRNNo = invoiceno;

        //            if (buyer.Address == null)
        //            {
        //                buyer.Address = new CreateOrEditSalesInvoiceAddressDto();
        //            }
        //            if (buyer.ContactPerson == null)
        //            {
        //                buyer.ContactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
        //            }

        //            buyer.Address.Type = "Buyer";
        //            buyer.ContactPerson.Type = "Buyer";
        //            buyer.Type = "Buyer";
        //        }

        //        foreach (var item in input.Items)
        //        {
        //            item.IRNNo = invoiceno;
        //            if (string.IsNullOrWhiteSpace(item.VATCode))
        //            {
        //                if (item.VATRate == 15)
        //                    item.VATCode = "S";
        //                else if (item.VATRate == 0)
        //                    item.VATCode = "Z";
        //                else
        //                    item.VATCode = "S";
        //            }
        //        }

        //        string errors = await Validator(input, batchId);
        //        connStr = _dbContextProvider.GetDbContext().Database.GetConnectionString();
        //        if (errors != null && errors.Length > 0)
        //            await UpdateInvoiceStatus(new InvoiceStatusType()
        //            {
        //                status = "R",
        //                invoiceType = "Sales",
        //                irnno = invoiceno,
        //                batchId = batchId,
        //                errors = errors,
        //                refNo = input.InvoiceNumber,
        //                inputData = JsonConvert.SerializeObject(input),
        //                isPdfGenerated = false,
        //                isXmlSigned = false,
        //                TenantId = AbpSession.TenantId
        //            });
        //        else
        //            await UpdateInvoiceStatus(new InvoiceStatusType()
        //            {
        //                status = "V",
        //                invoiceType = "Sales",
        //                irnno = invoiceno,
        //                batchId = batchId,
        //                errors = errors,
        //                refNo = input.InvoiceNumber,
        //                inputData = JsonConvert.SerializeObject(input),
        //                isPdfGenerated = false,
        //                isXmlSigned = false,
        //                TenantId = AbpSession.TenantId
        //            });

        //        await CreateOrEdit(input);
        //        await _invoiceSummariesAppService.CreateOrEdit(input.InvoiceSummary);

        //        foreach(var buyer in input.Buyer)
        //        {
        //            await _partyAppService.CreateOrEdit(buyer);
        //            await _invoiceAddressesAppService.CreateOrEdit(buyer.Address);
        //            await _contactPersonsAppService.CreateOrEdit(buyer.ContactPerson);
        //        }
        //        await _partyAppService.CreateOrEdit(input.Supplier[0]);
        //        await _invoiceAddressesAppService.CreateOrEdit(input.Supplier[0].Address);
        //        await _contactPersonsAppService.CreateOrEdit(input.Supplier[0].ContactPerson);


        //        //--------------------newly added---------------------------
        //        if (input.PaymentDetails == null || input.PaymentDetails.Count == 0)
        //        {
        //            var paymentD = new CreateOrEditSalesInvoicePaymentDetailDto()
        //            {
        //                PaymentMeans = "Cash",
        //                PaymentTerms = "",
        //                IRNNo = invoiceno

        //            };
        //            input.PaymentDetails = new List<CreateOrEditSalesInvoicePaymentDetailDto>();
        //            input.PaymentDetails.Add(paymentD);
        //            await _paymentDetailsAppService.CreateOrEdit(paymentD);

        //        }
        //        else
        //        {
        //            foreach (var paymentDetail in input.PaymentDetails)
        //            {
        //                paymentDetail.IRNNo = invoiceno;
        //                await _paymentDetailsAppService.CreateOrEdit(paymentDetail);
        //            }
        //        }

        //        if (input.VATDetails == null || input.VATDetails.Count == 0)
        //        {
        //            var details = input.Items.Select(p => p.VATCode).Distinct().ToList();
        //            input.VATDetails = new List<CreateOrEditSalesInvoiceVATDetailDto>();

        //            foreach (var item in details)
        //            {
        //                var vatdata = new CreateOrEditSalesInvoiceVATDetailDto();
        //                vatdata.IRNNo = invoiceno;
        //                vatdata.VATRate = (decimal)input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATRate;
        //                vatdata.TaxSchemeId = "VAT";
        //                vatdata.ExcemptionReasonCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonCode;
        //                vatdata.ExcemptionReasonText = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonText;
        //                vatdata.VATCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATCode;
        //                vatdata.TaxAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.VATAmount);
        //                vatdata.TaxableAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.NetPrice);
        //                vatdata.CurrencyCode = "SAR";
        //                input.VATDetails.Add(vatdata);
        //                await _vatDetailsAppService.CreateOrEdit(vatdata);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var vatDetail in input.VATDetails)
        //            {
        //                vatDetail.IRNNo = invoiceno;
        //                await _vatDetailsAppService.CreateOrEdit(vatDetail);
        //            }
        //        }

        //        //------------------------------------------------------

        //        foreach (var item in input.Items)
        //        {
        //            await _invoiceItemsAppService.CreateOrEdit(item);
        //        }
        //        await unitOfWork.CompleteAsync();

        //    }


        //    // _dbContextProvider.GetDbContext().Database.CommitTransaction();

        //    var response = new InvoiceResponse();
        //    try
        //    {
        //        response.PdfFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
        //        response.XmlFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".xml";
        //        response.QRCodeUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".png";


        //        _ = Task.Run(() =>
        //            XmlPdfJob(input, data.UniqueIdentifier, invoiceno, batchId, isPhase1)
        //        );
        //        Guid uuid = data.UniqueIdentifier;
        //        //await _generateXmlAppService.GenerateXmlRequest_Invoice(input, invoiceno, uuid.ToString(), AbpSession.TenantId.ToString(),input.Additional_Info);
        //        //await UpdateInvoiceStatus("X", batchId, input.InvoiceNumber, 3);

        //        //var pathToSave = string.Empty;
        //        //if (AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "")
        //        //    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + AbpSession.TenantId, uuid.ToString());
        //        //else
        //        //    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", uuid.ToString());
        //        //if (!Directory.Exists(pathToSave))
        //        //    Directory.CreateDirectory(pathToSave);
        //        //var xmlfileName = input.Supplier[0].VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
        //        //var path = (Path.Combine(pathToSave, xmlfileName));
        //        //await _pdfReportAppService.GetPDFFile_Invoice(input, invoiceno, uuid.ToString(), AbpSession.TenantId.ToString());
        //        //await UpdateInvoiceStatus("I", batchId, input.InvoiceNumber, 4);

        //        response.InvoiceNumber = invoiceno;
        //        response.Uuid = data.UniqueIdentifier;

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.ToString());
        //    }
        //    //  await _dbContextProvider.GetDbContext().SaveChangesAsync();
        //    return null;

        //}

        public async void XmlPdfJob(CreateOrEditSalesInvoiceDto input, Guid uuid, string invoiceno, int batchId, bool isPhase1)
        {
            try
            {
                using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    var TenantId = AbpSession.TenantId;

                    await _generateXmlAppService.GenerateXmlRequest(input, new UblSharp.Dtos.XMLRequestParam
                    {
                        invoiceno = invoiceno,
                        uniqueIdentifier = uuid.ToString(),
                        tenantId = TenantId.ToString(),
                        xml_uid = input.Additional_Info,
                        isPhase1 = isPhase1,
                        invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Sales
                    });

                    using (var uow = UnitOfWorkManager.Begin())
                    {
                        await UpdateInvoiceStatus(new InvoiceStatusType()
                        {
                            status = "X",
                            invoiceType = "Sales",
                            irnno = invoiceno,
                            batchId = batchId,
                            refNo = input.InvoiceNumber,
                            inputData = JsonConvert.SerializeObject(input),
                            isPdfGenerated = false,
                            isXmlSigned = true,
                            TenantId = TenantId
                        });
                        await uow.CompleteAsync();
                    }

                    await _pdfReportAppService.GeneratePdfRequest(input, invoiceno, uuid.ToString(), TenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Sales);


                    using (var uow = UnitOfWorkManager.Begin())
                    {
                        await UpdateInvoiceStatus(new InvoiceStatusType()
                        {
                            status = "I",
                            invoiceType = "Sales",
                            irnno = invoiceno,
                            batchId = batchId,
                            refNo = input.InvoiceNumber,
                            inputData = JsonConvert.SerializeObject(input),
                            isPdfGenerated = true,
                            isXmlSigned = true,
                            TenantId = TenantId
                        });
                        await uow.CompleteAsync();
                    }

                    await unitOfWork.CompleteAsync();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }



        public async Task<bool> InsertBatchUploadSales(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate, bool isVita = true)
        {
            fromDate = _timeZoneConverter.Convert(fromDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromDate;
            toDate = _timeZoneConverter.Convert(toDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? toDate;
             InsertUploadDatatoLogs("FileReachedatSP", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

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
                        cmd.CommandText = isVita ? "InsertBatchUploadSales" : "InsertBatchUploadEinvocing";
                        cmd.CommandTimeout = 0;

                        json = json.Replace(@"\r\n", " ").Replace(@"\n", " ").Replace(@"\t", " ");

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);


                        await cmd.ExecuteNonQueryAsync();
                        InsertUploadDatatoLogs("FileReachedatDataBase", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
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

        public async Task<DataTable> GetFileMappings()
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
                        cmd.CommandText = "GetFileMappings";
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

        public async Task<string> GetFileMappingById(int id)
        {
            string json = null;
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
                        cmd.CommandText = "GetFileMappingById";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@id", id);

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            json = reader.GetString(0);
                            break;
                        }
                        conn.Close();

                        return json;

                    }
                 
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

      

        public async Task<bool> CreateOrUpdateFileMappings(FileMappingPost input)
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
                        cmd.CommandText = "CreateOrUpdateFileMappings";
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@json", input.json);
                        cmd.Parameters.AddWithValue("@type", input.type);
                        cmd.Parameters.AddWithValue("@name", input.name);
                        cmd.Parameters.AddWithValue("@id", input.id);
                        cmd.Parameters.AddWithValue("@isActive", input.isActive);

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

        public async Task<bool> DeleteFileMapping(int id)
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
                        cmd.CommandText = "DeleteFileMappings";
                        cmd.Parameters.AddWithValue("@id", id);

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

        public async Task<bool> UpdateMasterValidation(string? status)
        {
            DataTable dt = new DataTable();
            bool validstatus = false;
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
                        cmd.CommandText = "UpdateMasterValidation";
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            if (dataReader.Read())
                            {
                                validstatus = Convert.ToBoolean(dataReader["ValidStat"]);//dataReader.ToString('ValidStat');
                            }
                        }
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return validstatus;
                    }
                }

                return validstatus;
            }
            catch (Exception e)
            {
                return validstatus;
            }
        }

        public async Task<DataTable> GetFinancialYear()
        {
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
                        cmd.CommandText = "GetFinancialYear";
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                }

                return dt;
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> UpdateFinancialYear(string finYear)
        {
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
                        cmd.CommandText = "UpdateFinancialYear";
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddWithValue("@finyear", finYear);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return dt;
                    }
                }

                return dt;
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

        public async Task<DataTable> getintegrationdashboarddataasJson(DateTime fromDate, DateTime toDate, string type,
            string invoicereferencenumber,
                string invoicereferncedate,
                string purchaseOrderNo,
                string customername,
                string activestatus,
                string currency,
                string payernumber,
                string salesordernumber,
                string shiptonumber,
                string IRNo,
                string createdby)
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
                        cmd.CommandText = "getintegrationdashboarddataasJson";
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@invoicereferencenumber", invoicereferencenumber);
                        cmd.Parameters.AddWithValue("@invoicereferncedate", invoicereferncedate);
                        cmd.Parameters.AddWithValue("@purchaseOrderNo", purchaseOrderNo);
                        cmd.Parameters.AddWithValue("@customername", customername);
                        cmd.Parameters.AddWithValue("@activestatus", activestatus);
                        cmd.Parameters.AddWithValue("@currency", currency);
                        cmd.Parameters.AddWithValue("@payernumber", payernumber);
                        cmd.Parameters.AddWithValue("@salesorderno", salesordernumber);
                        cmd.Parameters.AddWithValue("@shiptono", shiptonumber);
                        cmd.Parameters.AddWithValue("@irnno", IRNo);
                        cmd.Parameters.AddWithValue("@createdby", createdby);
                            

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

        public async Task<DataTable> GetStatsDashboardData(DateTime fromDate, DateTime toDate)
        {
            if (fromDate.Year == 1 || toDate.Year == 1)
            {
                fromDate = DateTime.Now;
                toDate = DateTime.Now;
            }
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

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
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

        public async Task<DataTable> GetSummaryDashboardData(DateTime fromDate, DateTime toDate, string type)
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

        public async Task<DataTable> getInvoiceSuggestions(string irrno,string refNo)
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
                        cmd.Parameters.AddWithValue("@refNo", refNo);
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

        public async Task<DataTable> getPurchaseSuggestions(string irrno)
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
                        cmd.CommandText = "getPurchaseSuggestions";
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
        public async Task<DataTable> getsalesdetails(long irrno, string refNo)
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
                        cmd.Parameters.AddWithValue("@refNo",refNo);
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

        public async Task<DataTable> getExemptionReason(string vatcode)
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
                        cmd.CommandText = "getExemptionReason";
                        cmd.Parameters.AddWithValue("@vatcode", vatcode);


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

        public async Task<DataTable> getPurchaseDetails(int irrno)
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
                        cmd.CommandText = "getPurchasedetails";

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

        public async Task<DataTable> getsalesitemdetail(int irrno,string type)
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

        public async Task<DataTable> getpurchaseitemdetail(int irrno)
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
                        cmd.CommandText = "getpurchaseitemdetail";

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

        public async Task<DataTable> GetSalesData(DateTime fromDate, DateTime toDate, DateTime? creationDate, string customername, string salesorderno, string purchaseorderno, string invoicerefno, string buyercode, string shippedcode,string IRNo,string createdby)
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
                        cmd.Parameters.AddWithValue("@creationDate", creationDate);
                        cmd.Parameters.AddWithValue("@customername", customername);
                        cmd.Parameters.AddWithValue("@salesorderno", salesorderno);
                        cmd.Parameters.AddWithValue("@purchaseorderno", purchaseorderno);
                        cmd.Parameters.AddWithValue("@invoicerefno", invoicerefno);
                        cmd.Parameters.AddWithValue("@buyercode", buyercode);
                        cmd.Parameters.AddWithValue("@shippedcode", shippedcode);
                        cmd.Parameters.AddWithValue("@IRNo", IRNo);
                        cmd.Parameters.AddWithValue("@createdby", createdby);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<DataTable> ViewInvoice(string irrno,string type)
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
                        cmd.CommandText = "ViewInvoice";
                        cmd.Parameters.AddWithValue("@irrno", irrno);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                return dt;
            }
        }

        public async Task<Tuple<int,string>> InvoiceFromDraft(string IRNNo)
        {
            int invcNo = 0;
            string transType = "";
            int result = 1;
            int tenantid = Convert.ToInt32(AbpSession.TenantId);

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
                            cmd.CommandText = "CreateInvoiceFromDraft";
                            cmd.Parameters.AddWithValue("@draftIrnno", IRNNo);
                            cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);



                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                invcNo = reader["invcNo"] == DBNull.Value ? 1 :  (int)Convert.ToUInt64(reader["invcNo"]);
                                transType = Convert.ToString(reader["transType"]);
                                result = reader["result"]==DBNull.Value ? 1 : (int)Convert.ToInt64(reader["result"]);
                        }
                            conn.Close();
                           
                            
                    }
                    // await GetSalesInvoiceData(invcNo,tenantid, transType);
                    if(result == 0)
                    {

                        throw new Exception(transType);
                        
                    }
                 
                        return new Tuple<int, string>(invcNo, transType);

                    
                }
                

            }
            catch (Exception e)
            {
                    if (e.Message == "381")
                    {
                        throw new UserFriendlyException("Credit Note Amount Cannot Be Greater Than SalesInvoice Amount.");
                    }
                    if (e.Message == "383")
                    {
                    throw new UserFriendlyException("Debit Note Amount Cannot Be Greater Than SalesInvoice Amount.");
                      }
                return new Tuple<int, string>(invcNo, transType);

            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }


        }

        public async Task<Boolean> CreateInvoiceFromDraft(string IRNNo)
        {
            Tuple<int, string> data;
            InvoiceResponse response = new InvoiceResponse();;
            using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                 data= await InvoiceFromDraft(IRNNo);
                await unitOfWork.CompleteAsync();
            }
            response=await GetSalesInvoiceData(data.Item1, Convert.ToInt32(AbpSession.TenantId), data.Item2);
            if (data.Item2 == "388")
            {
                await UpdateInvoiceURL(response, "Sales");
                await InsertSalesReportData(data.Item1);

            }
            if (data.Item2 == "381")
            {
                await UpdateInvoiceURL(response, "Credit");
                await InsertCreditReportData(data.Item1);

            }
            if (data.Item2 == "383")
            {
                await UpdateInvoiceURL(response, "Debit");
                await InsertDebitReportData(data.Item1);

            }

            //   await CurrentUnitOfWork.SaveChangesAsync();


            return true;

        }

        public async Task<bool> InsertDebitReportData(long IRNNo)
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
                        cmd.CommandText = "InsertDebitReportData";

                        cmd.Parameters.AddWithValue("@IRNNo", IRNNo);
                        cmd.Parameters.AddWithValue("@TenantId", AbpSession.TenantId);


                        int i = cmd.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public async Task<bool> InsertCreditReportData(long IRNNo)
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
                        cmd.CommandText = "InsertCreditReportData";

                        cmd.Parameters.AddWithValue("@IRNNo", IRNNo);
                        cmd.Parameters.AddWithValue("@TenantId", AbpSession.TenantId);


                        int i = cmd.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<InvoiceResponse> GetSalesInvoiceData(int? irnNo, int tenantId, string transType)
        {
            bool isPhase1 = true;

            DataTable dt = new DataTable();
            dt = await _tenantbasicdetails.GetTenantById(tenantId);

            InvoiceResponse pdfResponse = new();
            IRNMaster irn = new();
            CreateOrEditSalesInvoiceDto data = new();
            CreateOrEditSalesInvoicePartyDto supplier = new();
            supplier.Address = new();
            supplier.ContactPerson = new();
            var i = 0;
            foreach (DataRow row in dt.Rows)
            {
                supplier.RegistrationName = row["TenancyName"].ToString();

                supplier.VATID = row["vatid"].ToString();
                supplier.Website = row["website"].ToString();
                supplier.FaxNo = row["faxNo"].ToString();
                //Address
                supplier.Address.AdditionalNo = row["AdditionalBuildingNumber"].ToString();
                supplier.Address.BuildingNo = row["BuildingNo"].ToString();
                supplier.Address.Street = row["Street"].ToString();
                supplier.Address.AdditionalStreet = row["AdditionalStreet"].ToString();
                supplier.Address.PostalCode = row["PostalCode"].ToString();
                supplier.Address.CountryCode = row["country"].ToString();
                supplier.Address.City = row["city"].ToString();
                supplier.Address.State = row["State"].ToString();
                supplier.CRNumber = row["DocumentNumber"].ToString();
                supplier.ContactPerson.ContactNumber = row["ContactNumber"].ToString();
                isPhase1 = Convert.ToInt32(row["isPhase1"] ?? 0) == 1;

                i++;
            }


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
                        cmd.CommandText = "GetSalesInvoiceData";
                        cmd.Parameters.AddWithValue("@IrnNo", irnNo);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@transtype", transType);
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            if (dataReader.Read())
                            {
                                irn = dataReader.MapToObject<IRNMaster>();
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    data = dataReader.MapToObject<CreateOrEditSalesInvoiceDto>();
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Items == null)
                                        data.Items = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceItemDto>();
                                    data.Items.Add(items);
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.InvoiceSummary == null)
                                        data.InvoiceSummary = new();
                                    data.InvoiceSummary = dataReader.MapToObject<CreateOrEditSalesInvoiceSummaryDto>();
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer == null)
                                        data.Buyer = new();
                                    //if (data.Supplier == null)
                                    //   supplier = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoicePartyDto>();
                                    if (party.Type == "Buyer" )
                                    {
                                        data.Buyer.Add(party);
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier.Add(party);
                                    //}
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer[0].Address == null)
                                        data.Buyer[0].Address = new();
                                    //if (data.Supplier[0].Address == null)
                                    //   supplier[0].Address = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoiceAddressDto>();
                                    if (party.Type == "Buyer")
                                    {
                                        data.Buyer[0].Address = party;
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier[0].Address = party;
                                    //}
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Buyer[0].ContactPerson == null)
                                        data.Buyer[0].ContactPerson = new();
                                    //if (data.Supplier[0].ContactPerson == null)
                                    //   supplier[0].ContactPerson = new();
                                    var party = dataReader.MapToObject<CreateOrEditSalesInvoiceContactPersonDto>();
                                    if (party.Type == "Buyer")
                                    {
                                        data.Buyer[0].ContactPerson = party;
                                    }
                                    //else if (party.Type == "Supplier" && string.IsNullOrEmpty(party.Language))
                                    //{
                                    //   supplier[0].ContactPerson = party;
                                    //}
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.VATDetails == null)
                                        data.VATDetails = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceVATDetailDto>();
                                    data.VATDetails.Add(items);
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Discount == null)
                                        data.Discount = new();
                                    var items = dataReader.MapToObject<CreateOrEditSalesInvoiceDiscountDto>();
                                    data.Discount.Add(items);
                                }
                            }
                            data.Supplier = new();
                            data.Supplier.Add(supplier);

                            var response = new InvoiceResponse();
                            try
                            {
                                response.PdfFileUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".pdf";
                                response.XmlFileUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".xml";
                                response.QRCodeUrl = irn.UniqueIdentifier + "_" + irn.IRNNo.ToString() + ".png";

                                if (transType == "388")
                                {
                                    await _generateXmlAppService.GenerateXmlRequest(data, new UblSharp.Dtos.XMLRequestParam
                                    {
                                        invoiceno = irn.IRNNo.ToString(),
                                        uniqueIdentifier = irn.UniqueIdentifier.ToString(),
                                        tenantId = tenantId.ToString(),
                                        xml_uid = data.Additional_Info,
                                        isPhase1 = isPhase1,
                                        invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Sales
                                    });
                                }

                                if (transType == "383")
                                {
                                    await _generateXmlAppService.GenerateXmlRequest(data, new UblSharp.Dtos.XMLRequestParam
                                    {
                                        invoiceno = irn.IRNNo.ToString(),
                                        uniqueIdentifier = irn.UniqueIdentifier.ToString(),
                                        tenantId = tenantId.ToString(),
                                        xml_uid = data.Additional_Info,
                                        isPhase1 = isPhase1,
                                        invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Debit
                                    });
                                }

                                if (transType == "381")
                                {
                                    await _generateXmlAppService.GenerateXmlRequest(data, new UblSharp.Dtos.XMLRequestParam
                                    {
                                        invoiceno = irn.IRNNo.ToString(),
                                        uniqueIdentifier = irn.UniqueIdentifier.ToString(),
                                        tenantId = tenantId.ToString(),
                                        xml_uid = data.Additional_Info,
                                        isPhase1 = isPhase1,
                                        invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Credit
                                    });
                                }
                                //var newURL = Request.Scheme + "://" + Request.Host.Value + "/";

                                var pathToSave = string.Empty;
                                if (tenantId != null && tenantId.ToString() != "")
                                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + tenantId, irn.UniqueIdentifier.ToString());
                                else
                                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", irn.UniqueIdentifier.ToString());
                                if (!Directory.Exists(pathToSave))
                                    Directory.CreateDirectory(pathToSave);
                                var xmlfileName = data.Supplier[0].VATID + "_" + data.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + data.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + irn.IRNNo.ToString() + ".xml";
                                var path = (Path.Combine(pathToSave, xmlfileName));

                                var xmlBase64 = FileIO.GetFileInBas64(path);
                                var pdfBase64 = xmlBase64;
                                var xmlHash = FileIO.GetSha256FileHash(path);
                                var pdfHash = xmlHash;

                                if (transType == "381")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Credit);
                                }
                                if (transType == "383")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Debit);
                                }
                                if (transType == "388")
                                {
                                    pdfResponse = await _pdfReportAppService.GeneratePdfRequest(data, irn.IRNNo.ToString(), irn.UniqueIdentifier.ToString(), tenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Sales);
                                }
                                response.InvoiceNumber = irn.IRNNo.ToString();

                                _ = Task.Run(() => AzureUpload.CopyFolderToAzureStorage(pathToSave, tenantId, irn.UniqueIdentifier.ToString()));

                                //var emaildata = mapper.Map<InvoiceRequest>(data);
                                //_ = Task.Run(() => EmailSender.SendEmail(pdfResponse.PdfFileUrl, emaildata));


                            }
                            catch (Exception ex)
                            {
                                return new InvoiceResponse();
                            }
                            return new InvoiceResponse()
                            {
                                InvoiceId = Convert.ToInt32(irn.IRNNo.ToString()),
                                InvoiceNumber = data.InvoiceNumber,
                                Uuid = irn.UniqueIdentifier,
                                PdfFileUrl = AzureUpload.GetBlobUrl(pdfResponse.PdfFileUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/")),
                                XmlFileUrl = AzureUpload.GetBlobUrl(pdfResponse.XmlFileUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/")),
                                QRCodeUrl = AzureUpload.GetBlobUrl(pdfResponse.QRCodeUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/"))
                            };
                        }

                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<InvoiceResponse> GenerateInvoice(CreateOrEditSalesInvoiceDto input)
        {
            InvoiceResponse pdfResponse = new();
            input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;
            bool isPhase1 = true;

            DataTable dt = new DataTable();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {
                input.Supplier[0].RegistrationName = row["TenancyName"].ToString();

                input.Supplier[0].VATID = row["vatid"].ToString();
                input.Supplier[0].Website = row["website"].ToString();
                input.Supplier[0].FaxNo = row["faxNo"].ToString();
                //Address
                input.Supplier[0].Address.AdditionalNo = row["AdditionalBuildingNumber"].ToString();
                input.Supplier[0].Address.BuildingNo = row["BuildingNo"].ToString();
                input.Supplier[0].Address.Street = row["Street"].ToString();
                input.Supplier[0].Address.AdditionalStreet = row["AdditionalStreet"].ToString();
                input.Supplier[0].Address.PostalCode = row["PostalCode"].ToString();
                input.Supplier[0].Address.CountryCode = row["country"].ToString();
                input.Supplier[0].Address.City = row["city"].ToString();
                input.Supplier[0].Address.State = row["State"].ToString();
                input.Supplier[0].CRNumber = row["DocumentNumber"].ToString();
                input.Supplier[0].ContactPerson.ContactNumber = row["ContactNumber"].ToString();

                i++;

            }
            isPhase1 =  tenantConfigurationAppService.GetTenantConfigurationByTransactionType("General").Result?.TenantConfiguration?.isPhase1 ?? true;

            input.Supplier[0].Address.Type = "Supplier";
            input.Supplier[0].ContactPerson.Type = "Supplier";
            input.Supplier[0].ContactPerson.Type = "Supplier";
            input.Supplier[0].Type = "Supplier";
            

            var a = new CreateOrEditIRNMasterDto()
            {
                TransactionType = "SalesInvoice",
            };

            var data = await _transactionsAppService.CreateOrEdit(a);
            string invoiceno = data.IRNNo.ToString();
            input.IRNNo = invoiceno;
            input.Supplier[0].IRNNo = invoiceno;
            input.Supplier[0].Address.IRNNo = invoiceno;
            input.Supplier[0].ContactPerson.IRNNo = invoiceno;
            input.InvoiceSummary.IRNNo = invoiceno;
            input.Additional_Info = data.UniqueIdentifier.ToString();
            foreach (var buyer in input.Buyer)
            {
                if (buyer.Address == null)
                {
                    buyer.Address = new CreateOrEditSalesInvoiceAddressDto();
                }
                if (buyer.ContactPerson == null)
                {
                    buyer.ContactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
                }
                buyer.IRNNo = invoiceno;
                buyer.Address.IRNNo = invoiceno;
                buyer.ContactPerson.IRNNo = invoiceno;


                if(buyer.Address.Type == null)
                {
                    buyer.Address.Type = "Buyer";
                }

                if (buyer.ContactPerson.Type == null)
                {
                    buyer.ContactPerson.Type = "Buyer";
                }
                if (buyer.Type == null)
                {
                    buyer.Type = "Buyer";
                }
            }


            await CreateOrEdit(input);
            await _invoiceSummariesAppService.CreateOrEdit(input.InvoiceSummary);
            foreach (var buyer in input.Buyer)
            {
                await _partyAppService.CreateOrEdit(buyer);
                await _invoiceAddressesAppService.CreateOrEdit(buyer.Address);
                if(buyer.Language =="EN" || buyer.Language == null)
                {
                    await _contactPersonsAppService.CreateOrEdit(buyer.ContactPerson);

                }
            }



            await _partyAppService.CreateOrEdit(input.Supplier[0]);
            await _invoiceAddressesAppService.CreateOrEdit(input.Supplier[0].Address);
            await _contactPersonsAppService.CreateOrEdit(input.Supplier[0].ContactPerson);


            //--------------------newly added---------------------------
            if (input.PaymentDetails == null || input.PaymentDetails.Count == 0)
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

            if (input.VATDetails == null || input.VATDetails.Count == 0)
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
                item.Description = item.Description.Replace("\r\n", "<br />").Replace("\n", "<br />");
                item.IRNNo = invoiceno;
                //if (string.IsNullOrWhiteSpace(item.VATCode))
                //{
                //    {
                //        if (item.VATRate == 15)
                //            item.VATCode = "S";
                //        else if (item.VATRate == 0)
                //            item.VATCode = "Z";
                //        else
                //            item.VATCode = "S";
                //    }
                //}
                await _invoiceItemsAppService.CreateOrEdit(item);


            }
            var response = new InvoiceResponse();
            try
            {
                response.PdfFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
                response.XmlFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".xml";
                response.QRCodeUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".png";

                await _generateXmlAppService.GenerateXmlRequest(input, new UblSharp.Dtos.XMLRequestParam
                {
                    invoiceno = invoiceno,
                    uniqueIdentifier = data.UniqueIdentifier.ToString(),
                    tenantId = AbpSession.TenantId.ToString(),
                    xml_uid = input.Additional_Info,
                    isPhase1 = isPhase1,
                    invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Sales
                });

                //var newURL = Request.Scheme + "://" + Request.Host.Value + "/";

                var pathToSave = string.Empty;
                if (AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + AbpSession.TenantId, data.UniqueIdentifier.ToString());
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", data.UniqueIdentifier.ToString());
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);
                var xmlfileName = input.Supplier[0].VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
                var path = (Path.Combine(pathToSave, xmlfileName));

                var xmlBase64 = FileIO.GetFileInBas64(path);
                var pdfBase64 = xmlBase64;
                var xmlHash = FileIO.GetSha256FileHash(path);
                var pdfHash = xmlHash;


                pdfResponse = await _pdfReportAppService.GeneratePdfRequest(input, invoiceno, data.UniqueIdentifier.ToString(), AbpSession.TenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Sales);

                int tenantId = AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "" ? (int)AbpSession.TenantId : 0;
                _ = Task.Run(() => AzureUpload.CopyFolderToAzureStorage(pathToSave, tenantId, data.UniqueIdentifier.ToString()));

                var emaildata = mapper.Map<InvoiceRequest>(input);
                var email = tenantConfigurationAppService.GetTenantConfigurationByTransactionType("General");

                if (email.Result.TenantConfiguration.EmailJson != null && email.Result.TenantConfiguration.EmailJson != "{}")
                {
                    var emailsetting = (EmailDto)JsonConvert.DeserializeObject(email.Result.TenantConfiguration.EmailJson, typeof(EmailDto));
                    _ = Task.Run(() => EmailSender.SendEmail(pdfResponse.PdfFileUrl, emaildata, emailsetting));
                }


                response.InvoiceNumber = invoiceno;


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
                PdfFileUrl = AzureUpload.GetBlobUrl(pdfResponse.PdfFileUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/")),
                XmlFileUrl = AzureUpload.GetBlobUrl(pdfResponse.XmlFileUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/")),
                QRCodeUrl = AzureUpload.GetBlobUrl(pdfResponse.QRCodeUrl.Replace("wwwroot\\", "").Replace("InvoiceFiles\\", "").Replace("\\", "/"))
            };

        }

        [VitaFilter_Validation(VitaFilter_ValidationType.Sales)]
        public async Task<InvoiceResponse> CreateSalesInvoice(CreateOrEditSalesInvoiceDto input)
        {

            InvoiceResponse data = new InvoiceResponse();

            // await _dbContextProvider.GetDbContext().SaveChangesAsync();
            // await _dbContextProvider.GetDbContext().Database.CommitTransactionAsync();
            //await _dbContextProvider.GetDbContext().Database.CloseConnectionAsync();
            using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                data = await GenerateInvoice(input);
                await unitOfWork.CompleteAsync();
            }
            await UpdateInvoiceURL(data, "Sales");

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

        public async Task<bool> CheckIfRefNumExists(string RefNum)
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
                        cmd.CommandText = "CheckIfRefNumExists";

                        cmd.Parameters.AddWithValue("@refnum", RefNum);
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);

                        var reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            count = reader.GetInt32("count");
                        }
                        conn.Close();

                        return count > 0;
                    }

                }
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public async Task<DataTable> GetBuyertypelist()
        {

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
                        cmd.CommandText = "getbuyertype";

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

        public Task<InvoiceResponse> GenerateInvoice_SG(CreateOrEditSalesInvoiceDto input, int batchId)
        {
            throw new NotImplementedException();
        }
    }
}