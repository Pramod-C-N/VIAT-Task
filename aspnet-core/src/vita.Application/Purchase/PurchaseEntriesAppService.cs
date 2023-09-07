using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Purchase.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using vita.Sales.Dtos;
using vita.MasterData;
using vita.PdfFile;
using vita.UblSharp;
using Abp.EntityFrameworkCore;
using vita.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Abp.Timing.Timezone;

namespace vita.Purchase
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntries)]
    public class PurchaseEntriesAppService : vitaAppServiceBase, IPurchaseEntriesAppService
    {
        private readonly IRepository<PurchaseEntry, long> _purchaseEntryRepository;
        private readonly IPurchaseEntryPartiesAppService _partyAppService;
        private readonly IPurchaseEntryItemsAppService _invoiceItemsAppService;
        private readonly IPurchaseEntryVATDetailsAppService _vatDetailsAppService;
        private readonly IPurchaseEntryPaymentDetailsAppService _paymentDetailsAppService;
        private readonly IPurchaseEntrySummariesAppService _invoiceSummariesAppService;
        private readonly IPurchaseEntryDiscountsAppService _discountAppService;
        private readonly IPurchaseEntryContactPersonsAppService _contactPersonsAppService;
        private readonly IPurchaseEntryAddressesAppService _invoiceAddressesAppService;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITimeZoneConverter _timeZoneConverter;


        public PurchaseEntriesAppService(IRepository<PurchaseEntry, long> purchaseEntryRepository,
            IPurchaseEntryPartiesAppService partyAppService,
       IPurchaseEntryItemsAppService invoiceItemsAppService,
       IPurchaseEntryVATDetailsAppService vatDetailsAppService,
       IPurchaseEntryPaymentDetailsAppService paymentDetailsAppService,
       IPurchaseEntrySummariesAppService invoiceSummariesAppService,
       IPurchaseEntryDiscountsAppService discountAppService,
       IPurchaseEntryContactPersonsAppService contactPersonsAppService,
       IPurchaseEntryAddressesAppService invoiceAddressesAppService,
       IIRNMastersAppService transactionsAppService,
       IGenerateXmlAppService generateXmlAppService,
       IPdfReportAppService pdfReportAppService,
       IDbContextProvider<vitaDbContext> dbContextProvider,
       ITimeZoneConverter timeZoneConverter)
        {
            _purchaseEntryRepository = purchaseEntryRepository;
            _contactPersonsAppService = contactPersonsAppService;
            _discountAppService = discountAppService;
            _invoiceAddressesAppService = invoiceAddressesAppService;
            _invoiceSummariesAppService = invoiceSummariesAppService;
            _invoiceItemsAppService = invoiceItemsAppService;
            _partyAppService = partyAppService;
            _vatDetailsAppService = vatDetailsAppService;
            _paymentDetailsAppService = paymentDetailsAppService;
            _dbContextProvider= dbContextProvider;
            _timeZoneConverter= timeZoneConverter;

        }



        public async Task<bool> InsertPurchaseReportData(long IRNNo)
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
                        cmd.CommandText = "InsertPurchaseReportData";

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
        public async Task<bool> InsertBatchUploadPurchase(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate)
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
                        cmd.CommandText = "InsertBatchUploadPurchase";

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

        public async Task<DataTable> GetPurchaseData(DateTime fromDate, DateTime toDate)
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
                        cmd.CommandText = "GetPurchaseData";
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
        public async Task<PagedResultDto<GetPurchaseEntryForViewDto>> GetAll(GetAllPurchaseEntriesInput input)
        {


            var filteredPurchaseEntries = _purchaseEntryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.InvoiceNumber.Contains(input.Filter) || e.InvoiceCurrencyCode.Contains(input.Filter) || e.CurrencyCodeOriginatingCountry.Contains(input.Filter) || e.PurchaseOrderId.Contains(input.Filter) || e.BillingReferenceId.Contains(input.Filter) || e.ContractId.Contains(input.Filter) || e.Location.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.Additional_Info.Contains(input.Filter) || e.PaymentType.Contains(input.Filter) || e.PdfUrl.Contains(input.Filter) || e.QrCodeUrl.Contains(input.Filter) || e.XMLUrl.Contains(input.Filter) || e.ArchivalUrl.Contains(input.Filter) || e.PreviousInvoiceHash.Contains(input.Filter) || e.PerviousXMLHash.Contains(input.Filter) || e.XMLHash.Contains(input.Filter) || e.PdfHash.Contains(input.Filter) || e.XMLbase64.Contains(input.Filter) || e.PdfBase64.Contains(input.Filter) || e.TransTypeDescription.Contains(input.Filter) || e.AdvanceReferenceNumber.Contains(input.Filter) || e.Invoicetransactioncode.Contains(input.Filter) || e.BusinessProcessType.Contains(input.Filter) || e.InvoiceNotes.Contains(input.Filter) || e.PurchaseNumber.Contains(input.Filter) || e.BillOfEntry.Contains(input.Filter) || e.PlaceofSupply.Contains(input.Filter))
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceNotesFilter), e => e.InvoiceNotes.Contains(input.InvoiceNotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseNumberFilter), e => e.PurchaseNumber.Contains(input.PurchaseNumberFilter))
                        .WhereIf(input.MinSupplierInvoiceDateFilter != null, e => e.SupplierInvoiceDate >= input.MinSupplierInvoiceDateFilter)
                        .WhereIf(input.MaxSupplierInvoiceDateFilter != null, e => e.SupplierInvoiceDate <= input.MaxSupplierInvoiceDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillOfEntryFilter), e => e.BillOfEntry.Contains(input.BillOfEntryFilter))
                        .WhereIf(input.MinBillOfEntryDateFilter != null, e => e.BillOfEntryDate >= input.MinBillOfEntryDateFilter)
                        .WhereIf(input.MaxBillOfEntryDateFilter != null, e => e.BillOfEntryDate <= input.MaxBillOfEntryDateFilter)
                        .WhereIf(input.MinCustomsPaidFilter != null, e => e.CustomsPaid >= input.MinCustomsPaidFilter)
                        .WhereIf(input.MaxCustomsPaidFilter != null, e => e.CustomsPaid <= input.MaxCustomsPaidFilter)
                        .WhereIf(input.MinCustomTaxFilter != null, e => e.CustomTax >= input.MinCustomTaxFilter)
                        .WhereIf(input.MaxCustomTaxFilter != null, e => e.CustomTax <= input.MaxCustomTaxFilter)
                        .WhereIf(input.IsWHTFilter.HasValue && input.IsWHTFilter > -1, e => (input.IsWHTFilter == 1 && e.IsWHT) || (input.IsWHTFilter == 0 && !e.IsWHT))
                        .WhereIf(input.VATDefferedFilter.HasValue && input.VATDefferedFilter > -1, e => (input.VATDefferedFilter == 1 && e.VATDeffered) || (input.VATDefferedFilter == 0 && !e.VATDeffered))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PlaceofSupplyFilter), e => e.PlaceofSupply.Contains(input.PlaceofSupplyFilter));

            var pagedAndFilteredPurchaseEntries = filteredPurchaseEntries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntries = from o in pagedAndFilteredPurchaseEntries
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
                                      o.PurchaseNumber,
                                      o.SupplierInvoiceDate,
                                      o.BillOfEntry,
                                      o.BillOfEntryDate,
                                      o.CustomsPaid,
                                      o.CustomTax,
                                      o.IsWHT,
                                      o.VATDeffered,
                                      o.PlaceofSupply,
                                      Id = o.Id
                                  };

            var totalCount = await filteredPurchaseEntries.CountAsync();

            var dbList = await purchaseEntries.ToListAsync();
            var results = new List<GetPurchaseEntryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntryForViewDto()
                {
                    PurchaseEntry = new PurchaseEntryDto
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
                        PurchaseNumber = o.PurchaseNumber,
                        SupplierInvoiceDate = o.SupplierInvoiceDate,
                        BillOfEntry = o.BillOfEntry,
                        BillOfEntryDate = o.BillOfEntryDate,
                        CustomsPaid = o.CustomsPaid,
                        CustomTax = o.CustomTax,
                        IsWHT = o.IsWHT,
                        VATDeffered = o.VATDeffered,
                        PlaceofSupply = o.PlaceofSupply,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPurchaseEntryForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntries_Edit)]
        public async Task<GetPurchaseEntryForEditOutput> GetPurchaseEntryForEdit(EntityDto<long> input)
        {
            var purchaseEntry = await _purchaseEntryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntryForEditOutput { PurchaseEntry = ObjectMapper.Map<CreateOrEditPurchaseEntryDto>(purchaseEntry) };

            return output;
        }

        public async Task<long> CreateOrEdit(CreateOrEditPurchaseEntryDto input)
        {
                return await Create(input);
        }

        public async Task<InvoiceResponse> CreatePurchaseEntry(CreateOrEditPurchaseEntryDto input)
        {

            InvoiceResponse data = new InvoiceResponse();

            // await _dbContextProvider.GetDbContext().SaveChangesAsync();
            // await _dbContextProvider.GetDbContext().Database.CommitTransactionAsync();
            //await _dbContextProvider.GetDbContext().Database.CloseConnectionAsync();
            using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                data = await GeneratePurchaseEntry(input);
                await unitOfWork.CompleteAsync();
            }

            //   await CurrentUnitOfWork.SaveChangesAsync();
            await InsertPurchaseReportData(data.InvoiceId);


            return data;
        }

            public async Task<InvoiceResponse> GeneratePurchaseEntry(CreateOrEditPurchaseEntryDto input)
        {
            input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;

            input.IRNNo = "0";
            input.Buyer.Address.Type = "Buyer";
            input.Buyer.ContactPerson.Type = "Buyer";
            input.Buyer.Type = "Buyer";
            input.Supplier.Address.Type = "Supplier";
            input.Supplier.ContactPerson.Type = "Supplier";
            input.Supplier.ContactPerson.Type = "Supplier";
            input.Supplier.Type = "Supplier";
            var data = await CreateOrEdit(input);

            string invoiceno = data.ToString();
            input.IRNNo = invoiceno;
            input.Buyer.IRNNo = invoiceno;
            input.Supplier.IRNNo = invoiceno;
            input.Supplier.Address.IRNNo = invoiceno;
            input.Buyer.Address.IRNNo = invoiceno;
            input.Buyer.ContactPerson.IRNNo = invoiceno;
            input.Supplier.ContactPerson.IRNNo = invoiceno;
            input.InvoiceSummary.IRNNo = invoiceno;

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
                var paymentD = new CreateOrEditPurchaseEntryPaymentDetailDto()
                {
                    PaymentMeans = "Cash",
                    PaymentTerms = "",
                    IRNNo = invoiceno

                };
                input.PaymentDetails = new List<CreateOrEditPurchaseEntryPaymentDetailDto>();
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
                input.VATDetails = new List<CreateOrEditPurchaseEntryVATDetailDto>();

                foreach (var item in details)
                {
                    var vatdata = new CreateOrEditPurchaseEntryVATDetailDto();
                    vatdata.IRNNo = invoiceno;
                    vatdata.VATRate = (decimal)input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATRate;
                    vatdata.TaxSchemeId = "VAT";
                    vatdata.ExcemptionReasonCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonCode;
                    vatdata.ExcemptionReasonText = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonText;
                    vatdata.VATCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATCode;
                    vatdata.TaxAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.VATAmount);
                    vatdata.TaxAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.NetPrice);
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
            //_dbContextProvider.GetDbContext().SaveChanges();
            //await InsertPurchaseReportData(data);
            return new InvoiceResponse()
            {
                InvoiceId = Convert.ToInt32(invoiceno),
                InvoiceNumber = input.InvoiceNumber,
                Uuid = Guid.NewGuid(),
            };


        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntries_Create)]
        protected virtual async Task<long> Create(CreateOrEditPurchaseEntryDto input)
        {
            var purchaseEntry = ObjectMapper.Map<PurchaseEntry>(input);
            purchaseEntry.UniqueIdentifier= Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseEntry.TenantId = (int?)AbpSession.TenantId;
            }

            return await _purchaseEntryRepository.InsertAndGetIdAsync(purchaseEntry);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntries_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntryDto input)
        {
            var purchaseEntry = await _purchaseEntryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntry);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntryRepository.DeleteAsync(input.Id);
        }

    }
}