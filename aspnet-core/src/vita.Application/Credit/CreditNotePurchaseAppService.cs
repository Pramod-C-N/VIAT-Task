﻿using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.Credit.Dtos;
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
using vita.Credit;
using Abp.Timing.Timezone;
using System.DirectoryServices.Protocols;

namespace vita.Credit
{
    [AbpAuthorize(AppPermissions.Pages_CreditNote)]
    public class CreditNotePurchaseAppService : vitaAppServiceBase, ICreditNotePurchaseAppService
    {
        private readonly IRepository<CreditNote, Guid> _creditNoteRepository;
        private readonly ICreditNotePartyAppService _partyAppService;
        private readonly ICreditNoteItemAppService _invoiceItemsAppService;
        private readonly ICreditNoteVATDetailAppService _vatDetailsAppService;
        private readonly ICreditNotePaymentDetailAppService _paymentDetailsAppService;
        private readonly ICreditNoteSummaryAppService _invoiceSummariesAppService;
        private readonly ICreditNoteDiscountAppService _discountAppService;
        private readonly ICreditNoteContactPersonAppService _contactPersonsAppService;
        private readonly ICreditNoteAddressAppService _invoiceAddressesAppService;
        private readonly IIRNMastersAppService _transactionsAppService;
        private readonly IGenerateXmlAppService _generateXmlAppService;
        private readonly IPdfReportAppService _pdfReportAppService;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITimeZoneConverter _timeZoneConverter;

        public CreditNotePurchaseAppService(IRepository<CreditNote, Guid> creditNoteRepository,
             ICreditNotePartyAppService partyAppService,
        ICreditNoteItemAppService invoiceItemsAppService,
        ICreditNoteVATDetailAppService vatDetailsAppService,
        ICreditNotePaymentDetailAppService paymentDetailsAppService,
        ICreditNoteSummaryAppService invoiceSummariesAppService,
        ICreditNoteDiscountAppService discountAppService,
        ICreditNoteContactPersonAppService contactPersonsAppService,
        ICreditNoteAddressAppService invoiceAddressesAppService,
        IIRNMastersAppService transactionsAppService,
        IGenerateXmlAppService generateXmlAppService,
        IPdfReportAppService pdfReportAppService,
        IDbContextProvider<vitaDbContext> dbContextProvider,
        ITimeZoneConverter timeZoneConverter)
        {
            _creditNoteRepository = creditNoteRepository;
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
            _timeZoneConverter = timeZoneConverter;


        }

        public async Task<DataTable> GetCreditData(DateTime fromDate, DateTime toDate, DateTime? creationDate, string customername, string salesorderno, string purchaseorderno, string invoicerefno, string buyercode, string shippedcode)
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
                        cmd.CommandText = "GetCreditData";
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

        public async Task<PagedResultDto<GetCreditNoteForViewDto>> GetAll(GetAllCreditNoteInput input)
        {

            var filteredCreditNote = _creditNoteRepository.GetAll()
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

            var pagedAndFilteredCreditNote = filteredCreditNote
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditNote = from o in pagedAndFilteredCreditNote
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

            var totalCount = await filteredCreditNote.CountAsync();

            var dbList = await creditNote.ToListAsync();
            var results = new List<GetCreditNoteForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCreditNoteForViewDto()
                {
                    CreditNote = new CreditNoteDto
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

            return new PagedResultDto<GetCreditNoteForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<bool> InsertBatchUploadCreditPurchase(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate)
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
                        cmd.CommandText = "InsertBatchUploadCreditPurchase";

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

        [AbpAuthorize(AppPermissions.Pages_CreditNote_Edit)]
        public async Task<GetCreditNoteForEditOutput> GetCreditNoteForEdit(EntityDto<Guid> input)
        {
            var creditNote = await _creditNoteRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCreditNoteForEditOutput { CreditNote = ObjectMapper.Map<CreateOrEditCreditNoteDto>(creditNote) };

            return output;
        }

        

        public async Task CreateOrEdit(CreateOrEditCreditNoteDto input)
        {
           await Create(input);
        }

        public async Task<InvoiceResponse> CreateCreditNote(CreateOrEditCreditNoteDto input)
        {

            input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;



 
            input.Supplier[0].Address.Type = "Supplier";
            input.Supplier[0].ContactPerson.Type = "Supplier";
            input.Supplier[0].ContactPerson.Type = "Supplier";
            input.Supplier[0].Type = "Supplier";

            var a = new CreateOrEditIRNMasterDto()
            {
                TransactionType = "CreditNote",
            };
            var data = await _transactionsAppService.CreateOrEdit(a);
            string invoiceno = data.IRNNo.ToString();
            input.IRNNo = invoiceno;
            input.Supplier[0].IRNNo = invoiceno;
            input.Supplier[0].Address.IRNNo = invoiceno;
            input.Supplier[0].ContactPerson.IRNNo = invoiceno;
            input.InvoiceSummary.IRNNo = invoiceno;
            foreach (var buyer in input.Buyer)
            {
                if (buyer.Address == null)
                {
                    buyer.Address = new CreateOrEditCreditNoteAddressDto();
                }
                if (buyer.ContactPerson == null)
                {
                    buyer.ContactPerson = new CreateOrEditCreditNoteContactPersonDto();
                }

                buyer.Address.Type = "Buyer";
                buyer.ContactPerson.Type = "Buyer";
                buyer.Type = "Buyer";
                buyer.IRNNo = invoiceno;
                buyer.Address.IRNNo = invoiceno;
                buyer.ContactPerson.IRNNo = invoiceno;
            }


            await CreateOrEdit(input);
            await _invoiceSummariesAppService.CreateOrEdit(input.InvoiceSummary);
            await _partyAppService.CreateOrEdit(input.Supplier[0]);
            foreach (var buyer in input.Buyer)
            {
                await _invoiceAddressesAppService.CreateOrEdit(buyer.Address);
                await _partyAppService.CreateOrEdit(buyer);
                await _contactPersonsAppService.CreateOrEdit(buyer.ContactPerson);
            }

            await _invoiceAddressesAppService.CreateOrEdit(input.Supplier[0].Address);
            await _contactPersonsAppService.CreateOrEdit(input.Supplier[0].ContactPerson);


            //--------------------newly added---------------------------
            if (input.PaymentDetails == null)
            {
                var paymentD = new CreateOrEditCreditNotePaymentDetailDto()
                {
                    PaymentMeans = "Cash",
                    PaymentTerms = "",
                    IRNNo = invoiceno

                };
                input.PaymentDetails = new List<CreateOrEditCreditNotePaymentDetailDto>();
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
                input.VATDetails = new List<CreateOrEditCreditNoteVATDetailDto>();

                foreach (var item in details)
                {
                    var vatdata = new CreateOrEditCreditNoteVATDetailDto();
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
                    invoiceType = EInvoicing.Dto.InvoiceTypeEnum.Credit
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


                var isGenFile = await _pdfReportAppService.GeneratePdfRequest(input, invoiceno, data.UniqueIdentifier.ToString(), AbpSession.TenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Credit);

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
            };
        }

        [AbpAuthorize(AppPermissions.Pages_CreditNote_Create)]
        protected virtual async Task Create(CreateOrEditCreditNoteDto input)
        {
            var creditNote = ObjectMapper.Map<CreditNote>(input);
            creditNote.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                creditNote.TenantId = (int?)AbpSession.TenantId;
            }

            await _creditNoteRepository.InsertAsync(creditNote);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNote_Edit)]
        protected virtual async Task Update(CreateOrEditCreditNoteDto input)
        {
            var creditNote = await _creditNoteRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, creditNote);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNote_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _creditNoteRepository.DeleteAsync(input.Id);
        }

    }
}