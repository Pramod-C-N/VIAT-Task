﻿using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Debit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using vita.MasterData;
using vita.PdfFile;
using vita.UblSharp;
using System.IO;
using vita.MasterData.Dtos;
using vita.Sales.Dtos;
using vita.Utils;
using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using vita.EntityFrameworkCore;
using vita.TenantDetails;
using System.Transactions;
using Abp.Timing.Timezone;
using System.Text.RegularExpressions;
using System.Text;
using vita.Credit.Dtos;
using NPOI.POIFS.Crypt.Dsig;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JsonFlatten;
using System.DirectoryServices.Protocols;
using vita.Filters;
using static vita.Filters.VitaFilter_Validation;
using vita.Sales;
using vita.TenantConfigurations;
using AutoMapper;
using vita.EInvoicing.Dto;
using vita.TenantConfigurations.Dtos;
using vita.Credit;

namespace vita.Debit
{
    [AbpAuthorize(AppPermissions.Pages_DebitNotes)]
    public class DebitNotesAppService : vitaAppServiceBase, IDebitNotesAppService
    {
        private readonly IRepository<DebitNote, long> _debitNoteRepository;
        private readonly IDebitNotePartiesAppService _partyAppService;
        private readonly IDebitNoteItemsAppService _invoiceItemsAppService;
        private readonly IDebitNoteVATDetailsAppService _vatDetailsAppService;
        private readonly IDebitNotePaymentDetailsAppService _paymentDetailsAppService;
        private readonly IDebitNoteSummariesAppService _invoiceSummariesAppService;
        private readonly IDebitNoteDiscountsAppService _discountAppService;
        private readonly IDebitNoteContactPersonsAppService _contactPersonsAppService;
        private readonly IDebitNoteAddressesAppService _invoiceAddressesAppService;
        private readonly IIRNMastersAppService _transactionsAppService;
        private readonly IGenerateXmlAppService _generateXmlAppService;
        private readonly IPdfReportAppService _pdfReportAppService;
        private readonly ITenantBasicDetailsAppService _tenantbasicdetails;
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly ISalesInvoicesAppService _salesInvoicesAppService;
        private readonly ITenantConfigurationAppService tenantConfigurationAppService;
        private readonly IMapper mapper;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly IRepository<SalesInvoiceItem, long> _salesInvoiceItemRepository;
        private readonly IRepository<DebitNoteItem, long> _debitItemRepository;
        public DebitNotesAppService(IRepository<DebitNote, long> debitNoteRepository,
            IDebitNotePartiesAppService partyAppService,
       IDebitNoteItemsAppService invoiceItemsAppService,
       IDebitNoteVATDetailsAppService vatDetailsAppService,
       IDebitNotePaymentDetailsAppService paymentDetailsAppService,
       IDebitNoteSummariesAppService invoiceSummariesAppService,
       IDebitNoteDiscountsAppService discountAppService,
       IDebitNoteContactPersonsAppService contactPersonsAppService,
       IDebitNoteAddressesAppService invoiceAddressesAppService,
       IIRNMastersAppService transactionsAppService,
       IGenerateXmlAppService generateXmlAppService,
       IPdfReportAppService pdfReportAppService,
       ITenantBasicDetailsAppService tenantbasicdetails,
       IDbContextProvider<vitaDbContext> dbContextProvider,
       ISalesInvoicesAppService salesInvoicesAppService,
              ITenantConfigurationAppService tenantConfigurationAppService,
                       IRepository<SalesInvoiceItem, long> salesInvoiceSummaryRepository,
         IRepository<DebitNoteItem, long> creditnoteitemRepository,
              IMapper mapper,
       ITimeZoneConverter timeZoneConverter)
        {
            _debitNoteRepository = debitNoteRepository;
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
            _salesInvoicesAppService = salesInvoicesAppService;
            this.tenantConfigurationAppService = tenantConfigurationAppService;
            this.mapper = mapper;
            _salesInvoiceItemRepository = salesInvoiceSummaryRepository;
            _debitItemRepository = creditnoteitemRepository;
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
        public async Task<DataTable> GetDebitData(DateTime fromDate, DateTime toDate, DateTime? creationDate, string customername, string salesorderno, string purchaseorderno, string invoicerefno, string buyercode, string shippedcode,string IRNo,string createdby)
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
                        cmd.CommandText = "GetDebitData";
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
                        cmd.Parameters.AddWithValue("@createdBy", createdby);

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
        public async Task<PagedResultDto<GetDebitNoteForViewDto>> GetAll(GetAllDebitNotesInput input)
        {

            var filteredDebitNotes = _debitNoteRepository.GetAll()
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Additional_InfoFilter), e => e.Additional_Info.Contains(input.Additional_InfoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTypeFilter), e => e.PaymentType.Contains(input.PaymentTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PdfUrlFilter), e => e.PdfUrl.Contains(input.PdfUrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QrCodeUrlFilter), e => e.QrCodeUrl.Contains(input.QrCodeUrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XMLUrlFilter), e => e.XMLUrl.Contains(input.XMLUrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ArchivalUrlFilter), e => e.ArchivalUrl.Contains(input.ArchivalUrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PreviousInvoiceHashFilter), e => e.PreviousInvoiceHash.Contains(input.PreviousInvoiceHashFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PerviousXMLHashFilter), e => e.PerviousXMLHash.Contains(input.PerviousXMLHashFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XMLHashFilter), e => e.XMLHash.Contains(input.XMLHashFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PdfHashFilter), e => e.PdfHash.Contains(input.PdfHashFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XMLbase64Filter), e => e.XMLbase64.Contains(input.XMLbase64Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PdfBase64Filter), e => e.PdfBase64.Contains(input.PdfBase64Filter))
                        .WhereIf(input.IsArchivedFilter.HasValue && input.IsArchivedFilter > -1, e => (input.IsArchivedFilter == 1 && e.IsArchived) || (input.IsArchivedFilter == 0 && !e.IsArchived))
                        .WhereIf(input.MinTransTypeCodeFilter != null, e => e.TransTypeCode >= input.MinTransTypeCodeFilter)
                        .WhereIf(input.MaxTransTypeCodeFilter != null, e => e.TransTypeCode <= input.MaxTransTypeCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransTypeDescriptionFilter), e => e.TransTypeDescription.Contains(input.TransTypeDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdvanceReferenceNumberFilter), e => e.AdvanceReferenceNumber.Contains(input.AdvanceReferenceNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoicetransactioncodeFilter), e => e.Invoicetransactioncode.Contains(input.InvoicetransactioncodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessProcessTypeFilter), e => e.BusinessProcessType.Contains(input.BusinessProcessTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceNotesFilter), e => e.InvoiceNotes.Contains(input.InvoiceNotesFilter));

            var pagedAndFilteredDebitNotes = filteredDebitNotes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNotes = from o in pagedAndFilteredDebitNotes
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
                                 o.Additional_Info,
                                 o.PaymentType,
                                 o.PdfUrl,
                                 o.QrCodeUrl,
                                 o.XMLUrl,
                                 o.ArchivalUrl,
                                 o.PreviousInvoiceHash,
                                 o.PerviousXMLHash,
                                 o.XMLHash,
                                 o.PdfHash,
                                 o.XMLbase64,
                                 o.PdfBase64,
                                 o.IsArchived,
                                 o.TransTypeCode,
                                 o.TransTypeDescription,
                                 o.AdvanceReferenceNumber,
                                 o.Invoicetransactioncode,
                                 o.BusinessProcessType,
                                 o.InvoiceNotes,
                                 Id = o.Id
                             };

            var totalCount = await filteredDebitNotes.CountAsync();

            var dbList = await debitNotes.ToListAsync();
            var results = new List<GetDebitNoteForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNoteForViewDto()
                {
                    DebitNote = new DebitNoteDto
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
                        Additional_Info = o.Additional_Info,
                        PaymentType = o.PaymentType,
                        PdfUrl = o.PdfUrl,
                        QrCodeUrl = o.QrCodeUrl,
                        XMLUrl = o.XMLUrl,
                        ArchivalUrl = o.ArchivalUrl,
                        PreviousInvoiceHash = o.PreviousInvoiceHash,
                        PerviousXMLHash = o.PerviousXMLHash,
                        XMLHash = o.XMLHash,
                        PdfHash = o.PdfHash,
                        XMLbase64 = o.XMLbase64,
                        PdfBase64 = o.PdfBase64,
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

            return new PagedResultDto<GetDebitNoteForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<bool> InsertBatchUploadDebitSales(string json, string fileName, int? tenantId, DateTime? fromdate, DateTime? todate)
        {

            fromdate = _timeZoneConverter.Convert(fromdate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? fromdate;
            todate = _timeZoneConverter.Convert(todate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? todate;
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
                        cmd.CommandText = "InsertBatchUploadDebitSales";

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@fromDate", fromdate);
                        cmd.Parameters.AddWithValue("@toDate", todate);



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

        public async Task<bool> InsertBatchUploadDebitPurchase(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate)
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
                        cmd.CommandText = "InsertBatchUploadDebitPurchase";

                        cmd.Parameters.AddWithValue("json", json);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);

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

        [AbpAuthorize(AppPermissions.Pages_DebitNotes_Edit)]
        public async Task<GetDebitNoteForEditOutput> GetDebitNoteForEdit(EntityDto<long> input)
        {
            var debitNote = await _debitNoteRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDebitNoteForEditOutput { DebitNote = ObjectMapper.Map<CreateOrEditDebitNoteDto>(debitNote) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDebitNoteDto input)
        {
            await Create(input);
        }

        [VitaFilter_Validation(VitaFilter_ValidationType.Debit)]
        public async Task<InvoiceResponse> CreateDebitNote(CreateOrEditDebitNoteDto input)
        {

            InvoiceResponse data = new InvoiceResponse();

            // await _dbContextProvider.GetDbContext().SaveChangesAsync();
            // await _dbContextProvider.GetDbContext().Database.CommitTransactionAsync();
            //await _dbContextProvider.GetDbContext().Database.CloseConnectionAsync();
            using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                data = await GenerateDebitNote(input);
                await unitOfWork.CompleteAsync();
            }
            await _salesInvoicesAppService.UpdateInvoiceURL(data, "Debit");

            //   await CurrentUnitOfWork.SaveChangesAsync();
            await InsertDebitReportData(data.InvoiceId);


            return data;
        }
        private async Task<InvoiceResponse> GenerateDebitNote(CreateOrEditDebitNoteDto input)
        {
            InvoiceResponse pdfResponse = new();
            input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;
            bool isPhase1 = true;

            DataTable dt = new DataTable();
            dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);
            var salesTotalAmount = ((_salesInvoiceItemRepository.GetAll().Where(a => a.IRNNo == input.BillingReferenceId).Sum(a => a.LineAmountInclusiveVAT)));
            var debitHeader = _debitNoteRepository.GetAll().Where(a => a.BillingReferenceId == input.BillingReferenceId);
            var debitItem = _debitItemRepository.GetAll();
            var total = from ch in debitHeader
                        from ci in debitItem
                        where ch.IRNNo == ci.IRNNo
                        select new
                        {
                            ci.IRNNo,
                            ci.LineAmountInclusiveVAT
                        };
            var creditCount = total.Count();

            // var creditTotalAmount = ((_creditSummaryRepository.GetAll().Where(a => a. == input.BillingReferenceId).Sum(a => a.LineAmountInclusiveVAT)));

            if ((_salesInvoiceItemRepository.GetAll().Where(a => a.IRNNo == input.BillingReferenceId)).Count() > 0 || creditCount > 0)
            {
                if (input.Items.Sum(a => a.LineAmountInclusiveVAT) > (salesTotalAmount - total.Sum(a => a.LineAmountInclusiveVAT)))
                {
                    throw new UserFriendlyException("Debit Note Amount Cannot Be Greater Than SalesInvoice Amount.");
                }
            }
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
                isPhase1 = Convert.ToInt32(row["isPhase1"] ?? 0) == 1;

                i++;
            }
            isPhase1 = tenantConfigurationAppService.GetTenantConfigurationByTransactionType("General").Result?.TenantConfiguration?.isPhase1 ?? true;

            input.Supplier[0].Address.Type = "Supplier";
            input.Supplier[0].ContactPerson.Type = "Supplier";
            input.Supplier[0].ContactPerson.Type = "Supplier";
            input.Supplier[0].Type = "Supplier";
            var a = new CreateOrEditIRNMasterDto()
            {
                TransactionType = "DebitNote",
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
                    buyer.Address = new CreateOrEditDebitNoteAddressDto();
                }
                if (buyer.ContactPerson == null)
                {
                    buyer.ContactPerson = new CreateOrEditDebitNoteContactPersonDto();
                }
                buyer.IRNNo = invoiceno;
                buyer.Address.IRNNo = invoiceno;
                buyer.ContactPerson.IRNNo = invoiceno;

                if (buyer.Address.Type == null)
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
                if (buyer.Language == "EN" || buyer.Language == null) {
                    await _contactPersonsAppService.CreateOrEdit(buyer.ContactPerson);
                }
                   
            }

            await _partyAppService.CreateOrEdit(input.Supplier[0]);
            await _invoiceAddressesAppService.CreateOrEdit(input.Supplier[0].Address);
            await _contactPersonsAppService.CreateOrEdit(input.Supplier[0].ContactPerson);

            //--------------------newly added---------------------------
            if (input.PaymentDetails == null || input.PaymentDetails.Count == 0)
            {
                var paymentD = new CreateOrEditDebitNotePaymentDetailDto()
                {
                    PaymentMeans = "Cash",
                    PaymentTerms = "",
                    IRNNo = invoiceno

                };
                input.PaymentDetails = new List<CreateOrEditDebitNotePaymentDetailDto>();
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
                input.VATDetails = new List<CreateOrEditDebitNoteVATDetailDto>();

                foreach (var item in details)
                {
                    var vatdata = new CreateOrEditDebitNoteVATDetailDto();
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


            foreach (var item in input.Items)
            {
                item.Description = item.Description.Replace("\r\n", "<br />").Replace("\n", "<br />");
                item.IRNNo = invoiceno;
                //if (string.IsNullOrWhiteSpace(item.VATCode))
                //{
                //    if (item.VATRate == 15)
                //        item.VATCode = "S";
                //    else if (item.VATRate == 0)
                //        item.VATCode = "Z";
                //    else
                //        item.VATCode = "S";
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
                    invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Debit
                });

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


                pdfResponse = await _pdfReportAppService.GeneratePdfRequest(input, invoiceno, data.UniqueIdentifier.ToString(), AbpSession.TenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Debit);

                int tenantId = AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "" ? (int)AbpSession.TenantId : 0;
                _ = Task.Run(() => AzureUpload.CopyFolderToAzureStorage(pathToSave, tenantId, data.UniqueIdentifier.ToString()));

                var emaildata = mapper.Map<InvoiceRequest>(input);
                var email = tenantConfigurationAppService.GetTenantConfigurationByTransactionType("General");

                if (email.Result.TenantConfiguration.EmailJson != null && email.Result.TenantConfiguration.EmailJson !="{}")
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

        public string[] uom = { "LTRS", "PCS", "NOS", "GMS", "KGS", "PACKS" };
        public string[] countryCode = { "SA" };
        public string[] taxCurrencyCode = { "SAR" };
        public string[] vatCode = { "S", "Z", "E", "O" };
        public async Task<string> Validator(CreateOrEditDebitNoteDto data, int batchId)
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
        //public async Task<bool> GenerateInvoice_SG(CreateOrEditDebitNoteDto input, int batchId)
        //{
        //    TransactionDto data = new TransactionDto();
        //    string invoiceno = "";

        //    using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
        //    {
        //        await unitOfWork.CompleteAsync();
        //        input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;

        //        input.InvoiceCurrencyCode = "SAR";
        //        DataTable dt = new DataTable();
        //        dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


        //        if (input.Supplier == null)
        //        {
        //            input.Supplier = new CreateOrEditDebitNotePartyDto();
        //            input.Supplier.Address = new CreateOrEditDebitNoteAddressDto();

        //        }
        //        input.Supplier.ContactPerson = new CreateOrEditDebitNoteContactPersonDto();

        //        if (input.Buyer == null)
        //        {
        //            input.Buyer = new CreateOrEditDebitNotePartyDto();
        //            input.Buyer.Address = new CreateOrEditDebitNoteAddressDto();
        //            input.Buyer.ContactPerson = new CreateOrEditDebitNoteContactPersonDto();

        //        }


        //        var i = 0;
        //        foreach (DataRow row in dt.Rows)
        //        {

        //            input.Supplier.VATID = row["vatid"].ToString();
        //            //Address
        //            input.Supplier.Address.AdditionalNo = row["AdditionalBuildingNumber"].ToString();
        //            input.Supplier.Address.BuildingNo = row["BuildingNo"].ToString();
        //            input.Supplier.Address.Street = row["Street"].ToString();
        //            input.Supplier.Address.AdditionalStreet = row["AdditionalStreet"].ToString();
        //            input.Supplier.Address.PostalCode = row["PostalCode"].ToString();
        //            input.Supplier.Address.CountryCode = row["country"].ToString();
        //            input.Supplier.Address.City = row["city"].ToString();
        //            input.Supplier.Address.State = row["State"].ToString();
        //            input.Supplier.CRNumber = row["DocumentNumber"].ToString();
        //            input.Supplier.ContactPerson.ContactNumber = row["ContactNumber"].ToString();

        //            i++;

        //        }
        //        input.Buyer.Address.Type = "Buyer";
        //        input.Buyer.ContactPerson.Type = "Buyer";
        //        input.Buyer.Type = "Buyer";
        //        input.Supplier.Address.Type = "Supplier";
        //        input.Supplier.ContactPerson.Type = "Supplier";
        //        input.Supplier.ContactPerson.Type = "Supplier";
        //        input.Supplier.Type = "Supplier";

        //        var a = new CreateOrEditIRNMasterDto()
        //        {
        //            TransactionType = "DebitNote",
        //        };

        //        data = await _transactionsAppService.CreateOrEdit(a);
        //        invoiceno = data.IRNNo.ToString();
        //        input.IRNNo = invoiceno;
        //        input.Buyer.IRNNo = invoiceno;
        //        input.Supplier.IRNNo = invoiceno;
        //        input.Supplier.Address.IRNNo = invoiceno;
        //        input.Buyer.Address.IRNNo = invoiceno;
        //        input.Buyer.ContactPerson.IRNNo = invoiceno;
        //        input.Supplier.ContactPerson.IRNNo = invoiceno;
        //        input.InvoiceSummary.IRNNo = invoiceno;

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

        //        if (errors != null && errors.Length > 0)
        //            await UpdateInvoiceStatus("R", batchId, input.InvoiceNumber, 0, invoiceno, errors);
        //        else
        //            await UpdateInvoiceStatus("V", batchId, input.InvoiceNumber, 1, invoiceno, errors);

        //        await CreateOrEdit(input);
        //        await _invoiceSummariesAppService.CreateOrEdit(input.InvoiceSummary);
        //        await _partyAppService.CreateOrEdit(input.Buyer);
        //        await _partyAppService.CreateOrEdit(input.Supplier);
        //        await _invoiceAddressesAppService.CreateOrEdit(input.Buyer.Address);
        //        await _invoiceAddressesAppService.CreateOrEdit(input.Supplier.Address);
        //        await _contactPersonsAppService.CreateOrEdit(input.Buyer.ContactPerson);
        //        await _contactPersonsAppService.CreateOrEdit(input.Supplier.ContactPerson);


        //        //--------------------newly added---------------------------
        //        if (input.PaymentDetails == null)
        //        {
        //            var paymentD = new CreateOrEditDebitNotePaymentDetailDto()
        //            {
        //                PaymentMeans = "Cash",
        //                PaymentTerms = "",
        //                IRNNo = invoiceno

        //            };
        //            input.PaymentDetails = new List<CreateOrEditDebitNotePaymentDetailDto>();
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

        //        if (input.VATDetails == null)
        //        {
        //            var details = input.Items.Select(p => p.VATCode).Distinct().ToList();
        //            input.VATDetails = new List<CreateOrEditDebitNoteVATDetailDto>();

        //            foreach (var item in details)
        //            {
        //                var vatdata = new CreateOrEditDebitNoteVATDetailDto();
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
        //    }


        //    // _dbContextProvider.GetDbContext().Database.CommitTransaction();

        //    var response = new InvoiceResponse();
        //    try
        //    {
        //        response.PdfFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
        //        response.XmlFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".xml";
        //        response.QRCodeUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".png";

        //        //   _ = Task.Run(() => XmlPdfJob(input,data.UniqueIdentifier,invoiceno,batchId));
        //        Guid uuid = data.UniqueIdentifier;
        //        await _generateXmlAppService.GenerateXmlRequest(input, new UblSharp.Dtos.XMLRequestParam
        //        {
        //            invoiceno = invoiceno,
        //            uniqueIdentifier = data.UniqueIdentifier.ToString(),
        //            tenantId = AbpSession.TenantId.ToString(),
        //            xml_uid = input.Additional_Info,
        //            invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Debit
        //        });

        //        await UpdateInvoiceStatus("X", batchId, input.InvoiceNumber, 3);

        //        var pathToSave = string.Empty;
        //        if (AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "")
        //            pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + AbpSession.TenantId, uuid.ToString());
        //        else
        //            pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", uuid.ToString());
        //        if (!Directory.Exists(pathToSave))
        //            Directory.CreateDirectory(pathToSave);
        //        var xmlfileName = input.Supplier.VATID + "_" + input.IssueDate.ToString("ddMMyyyy").Replace("-", String.Empty) + "T" + input.IssueDate.ToString("HH:mm:ss").Replace(":", String.Empty) + "_" + invoiceno + ".xml";
        //        var path = (Path.Combine(pathToSave, xmlfileName));
        //        await _pdfReportAppService.GeneratePdfRequest(input, invoiceno, data.UniqueIdentifier.ToString(), AbpSession.TenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Debit);

        //        await UpdateInvoiceStatus("I", batchId, input.InvoiceNumber, 4);



        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.ToString());
        //    }
        //    //  await _dbContextProvider.GetDbContext().SaveChangesAsync();
        //    return true;

        //}


        [AbpAuthorize(AppPermissions.Pages_DebitNotes_Create)]
        protected virtual async Task Create(CreateOrEditDebitNoteDto input)
        {
            var debitNote = ObjectMapper.Map<DebitNote>(input);
            debitNote.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                debitNote.TenantId = (int?)AbpSession.TenantId;
            }

            await _debitNoteRepository.InsertAsync(debitNote);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNotes_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNoteDto input)
        {
            var debitNote = await _debitNoteRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, debitNote);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNotes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _debitNoteRepository.DeleteAsync(input.Id);
        }


    }
}