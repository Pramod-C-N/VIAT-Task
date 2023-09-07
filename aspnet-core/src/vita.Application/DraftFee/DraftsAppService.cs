using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.DraftFee.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;
using AutoMapper;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using vita.EInvoicing.Dto;
using vita.MasterData.Dtos;
using vita.Sales.Dtos;
using vita.TenantConfigurations.Dtos;
using vita.TenantConfigurations;
using vita.TenantDetails;
using vita.Utils;
using static vita.Filters.VitaFilter_Validation;
using System.Transactions;
using vita.Filters;
using Abp.EntityFrameworkCore;
using Abp.Timing.Timezone;
using vita.EntityFrameworkCore;
using vita.MasterData;
using vita.PdfFile;
using vita.Sales;
using vita.UblSharp;
using NPOI.POIFS.Crypt.Dsig;
using System.Data.SqlClient;
using DbDataReaderMapper;

namespace vita.DraftFee
{
    [AbpAuthorize(AppPermissions.Pages_Drafts)]
    public class DraftsAppService : vitaAppServiceBase, IDraftsAppService
    {
        private readonly IRepository<Draft, long> _draftRepository;

        private readonly IRepository<SalesInvoice, long> _salesInvoiceRepository;
        private readonly IDraftPartiesAppService _partyAppService;
        private readonly IDraftItemsAppService _invoiceItemsAppService;
        private readonly IDraftVATDetailsAppService _vatDetailsAppService;
        private readonly IDraftPaymentDetailsAppService _paymentDetailsAppService;
        private readonly IDraftSummariesAppService _invoiceSummariesAppService;
        private readonly IDraftDiscountsAppService _discountAppService;
        private readonly IDraftContactPersonsAppService _contactPersonsAppService;
        private readonly IDraftAddressesAppService _invoiceAddressesAppService;
        private readonly IIRNMastersAppService _transactionsAppService;
        private readonly IPdfReportAppService _pdfReportAppService;
        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly ITenantBasicDetailsAppService _tenantbasicdetails;
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IMapper mapper;
        private readonly ITenantConfigurationAppService tenantConfigurationAppService;

        public DraftsAppService(
            IRepository<Draft, long> draftRepository,
            IRepository<SalesInvoice, long> salesInvoiceRepository,
            IDraftPartiesAppService partyAppService,
       IDraftItemsAppService invoiceItemsAppService,
       IDraftVATDetailsAppService vatDetailsAppService,
       IDraftPaymentDetailsAppService paymentDetailsAppService,
       IDraftSummariesAppService invoiceSummariesAppService,
       IDraftDiscountsAppService discountAppService,
       IDraftContactPersonsAppService contactPersonsAppService,
       IDraftAddressesAppService invoiceAddressesAppService,
       IIRNMastersAppService transactionsAppService,
       IPdfReportAppService pdfReportAppService,
       ITenantBasicDetailsAppService tenantbasicdetails,
       IDbContextProvider<vitaDbContext> dbContextProvider,
       ITimeZoneConverter timeZoneConverter,
       IMapper mapper,
       ITenantConfigurationAppService tenantConfigurationAppService)
        {
            _draftRepository = draftRepository;
            _salesInvoiceRepository = salesInvoiceRepository;
            _contactPersonsAppService = contactPersonsAppService;
            _discountAppService = discountAppService;
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

        public virtual async Task<PagedResultDto<GetDraftForViewDto>> GetAll(GetAllDraftsInput input)
        {

            var filteredDrafts = _draftRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.InvoiceNumber.Contains(input.Filter) || e.InvoiceCurrencyCode.Contains(input.Filter) || e.CurrencyCodeOriginatingCountry.Contains(input.Filter) || e.PurchaseOrderId.Contains(input.Filter) || e.BillingReferenceId.Contains(input.Filter) || e.ContractId.Contains(input.Filter) || e.Location.Contains(input.Filter) || e.CustomerId.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.Additional_Info.Contains(input.Filter) || e.PaymentType.Contains(input.Filter) || e.PdfUrl.Contains(input.Filter) || e.QrCodeUrl.Contains(input.Filter) || e.XMLUrl.Contains(input.Filter) || e.ArchivalUrl.Contains(input.Filter) || e.PreviousInvoiceHash.Contains(input.Filter) || e.PerviousXMLHash.Contains(input.Filter) || e.XMLHash.Contains(input.Filter) || e.PdfHash.Contains(input.Filter) || e.XMLbase64.Contains(input.Filter) || e.PdfBase64.Contains(input.Filter) || e.TransTypeDescription.Contains(input.Filter) || e.AdvanceReferenceNumber.Contains(input.Filter) || e.Invoicetransactioncode.Contains(input.Filter) || e.BusinessProcessType.Contains(input.Filter) || e.InvoiceNotes.Contains(input.Filter) || e.XmlUuid.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter) || e.AdditionalData2.Contains(input.Filter) || e.AdditionalData3.Contains(input.Filter) || e.AdditionalData4.Contains(input.Filter) || e.InvoiceTypeCode.Contains(input.Filter) || e.Language.Contains(input.Filter) || e.Error.Contains(input.Filter) || e.DraftStatus.Contains(input.Filter) || e.Source.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniqueIdentifierFilter.ToString()), e => e.UniqueIdentifier.ToString() == input.UniqueIdentifierFilter.ToString())
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XmlUuidFilter), e => e.XmlUuid.Contains(input.XmlUuidFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData1Filter), e => e.AdditionalData1.Contains(input.AdditionalData1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData2Filter), e => e.AdditionalData2.Contains(input.AdditionalData2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData3Filter), e => e.AdditionalData3.Contains(input.AdditionalData3Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData4Filter), e => e.AdditionalData4.Contains(input.AdditionalData4Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceTypeCodeFilter), e => e.InvoiceTypeCode.Contains(input.InvoiceTypeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageFilter), e => e.Language.Contains(input.LanguageFilter))
                        .WhereIf(input.isSentFilter.HasValue && input.isSentFilter > -1, e => (input.isSentFilter == 1 && e.isSent) || (input.isSentFilter == 0 && !e.isSent))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorFilter), e => e.Error.Contains(input.ErrorFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DraftStatusFilter), e => e.DraftStatus.Contains(input.DraftStatusFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SourceFilter), e => e.Source.Contains(input.SourceFilter));

            var pagedAndFilteredDrafts = filteredDrafts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var drafts = from o in pagedAndFilteredDrafts
                         select new
                         {

                             Id = o.Id
                         };

            var totalCount = await filteredDrafts.CountAsync();

            var dbList = await drafts.ToListAsync();
            var results = new List<GetDraftForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftForViewDto()
                {
                    Draft = new DraftDto
                    {

                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_Drafts_Edit)]
        public virtual async Task<GetDraftForEditOutput> GetDraftForEdit(EntityDto<long> input)
        {
            var draft = await _draftRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftForEditOutput { Draft = ObjectMapper.Map<CreateOrEditDraftDto>(draft) };

            return output;
        }

        public virtual async Task<(string, Guid)> CreateOrEdit(CreateOrEditDraftDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Drafts_Create)]
        protected virtual async Task<(string, Guid)> Create(CreateOrEditDraftDto input)
        {
            var draft = ObjectMapper.Map<Draft>(input);
            draft.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                draft.TenantId = (int?)AbpSession.TenantId;
            }

            string id = (await _draftRepository.InsertAndGetIdAsync(draft)).ToString();

            return (id, draft.UniqueIdentifier);

        }

        [AbpAuthorize(AppPermissions.Pages_Drafts_Edit)]
        protected virtual async Task<(string, Guid)> Update(CreateOrEditDraftDto input)
        {
            var draft = await _draftRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, draft);

            await _draftRepository.UpdateAsync(draft);
            //draft.UniqueIdentifier = Guid.NewGuid();
            return ((input.Id).ToString(), draft.UniqueIdentifier);

        }

        //[AbpAuthorize(AppPermissions.Pages_Drafts_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftRepository.DeleteAsync(input.Id);
        }

        [VitaFilter_Validation(VitaFilter_ValidationType.Sales)]
        public async Task<InvoiceResponse> CreateDraft(CreateOrEditDraftDto input)
        {
            InvoiceResponse data = new InvoiceResponse();
            using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                data = await GenerateDraft(input);
                await unitOfWork.CompleteAsync();
            }
            await UpdateInvoiceURL(data, "Draft");

            return data;

        }

        private async Task<bool> UpdateInvoiceURL(InvoiceResponse response, string type)
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



        private async Task<InvoiceResponse> GenerateDraft(CreateOrEditDraftDto input)
        {
            InvoiceResponse pdfResponse = new();
            input.IssueDate = _timeZoneConverter.Convert(input.IssueDate, AbpSession.TenantId, AbpSession.UserId ?? 0) ?? input.IssueDate;
            bool isPhase1 = true;

            DataTable dt = await _tenantbasicdetails.GetTenantById(AbpSession.TenantId);


            var i = 0;
            foreach (DataRow row in dt.Rows)
            {
                input.Supplier[0].RegistrationName = row["TenancyName"].ToString();

                input.Supplier[0].VATID = row["vatid"].ToString();
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
            isPhase1 = tenantConfigurationAppService.GetTenantConfigurationByTransactionType("General").Result?.TenantConfiguration?.isPhase1 ?? true;

            input.Supplier[0].Address.Type = "Supplier";
            input.Supplier[0].ContactPerson.Type = "Supplier";
            input.Supplier[0].ContactPerson.Type = "Supplier";
            input.Supplier[0].Type = "Supplier";


            var a = new CreateOrEditIRNMasterDto()
            {
                TransactionType = "SalesInvoice",
            };

            input.IRNNo = "-1";
            var data = input;
            (data.IRNNo, data.UniqueIdentifier) = await CreateOrEdit(input);
            string invoiceno = data.IRNNo;
            input.Supplier[0].IRNNo = invoiceno;
            input.Supplier[0].Address.IRNNo = invoiceno;
            input.Supplier[0].ContactPerson.IRNNo = invoiceno;
            input.InvoiceSummary.IRNNo = invoiceno;
            input.Additional_Info = data.UniqueIdentifier.ToString();
            foreach (var buyer in input.Buyer)
            {
                if (buyer.Address == null)
                {
                    buyer.Address = new CreateOrEditDraftAddressDto();
                }
                if (buyer.ContactPerson == null)
                {
                    buyer.ContactPerson = new CreateOrEditDraftContactPersonDto();
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

            await _invoiceSummariesAppService.CreateOrEdit(input.InvoiceSummary);
            foreach (var buyer in input.Buyer)
            {
                await _partyAppService.CreateOrEdit(buyer);
                await _invoiceAddressesAppService.CreateOrEdit(buyer.Address);
                if (buyer.Language == "EN" || buyer.Language == null)
                {
                    await _contactPersonsAppService.CreateOrEdit(buyer.ContactPerson);

                }
            }


            if (!input.Id.HasValue)
            {
                await _partyAppService.CreateOrEdit(input.Supplier[0]);
                await _invoiceAddressesAppService.CreateOrEdit(input.Supplier[0].Address);
                await _contactPersonsAppService.CreateOrEdit(input.Supplier[0].ContactPerson);
            }


            //--------------------newly added---------------------------
            if (input.PaymentDetails == null || input.PaymentDetails.Count == 0)
            {
                var paymentD = new CreateOrEditDraftPaymentDetailDto()
                {
                    PaymentMeans = "Cash",
                    PaymentTerms = "",
                    IRNNo = invoiceno

                };
                input.PaymentDetails = new List<CreateOrEditDraftPaymentDetailDto>();
                paymentD.Id = input.PaymentDetails.FirstOrDefault()?.Id;
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
                input.VATDetails = new List<CreateOrEditDraftVATDetailDto>();

                foreach (var item in details)
                {
                    var vatdata = new CreateOrEditDraftVATDetailDto();
                    vatdata.IRNNo = invoiceno;
                    vatdata.VATRate = (decimal)input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATRate;
                    vatdata.TaxSchemeId = "VAT";
                    vatdata.ExcemptionReasonCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonCode;
                    vatdata.ExcemptionReasonText = input.Items.Where(p => p.VATCode == item).FirstOrDefault().ExcemptionReasonText;
                    vatdata.VATCode = input.Items.Where(p => p.VATCode == item).FirstOrDefault().VATCode;
                    vatdata.TaxAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.VATAmount);
                    vatdata.TaxableAmount = input.Items.Where(p => p.VATCode == item).Sum(p => p.NetPrice);
                    vatdata.CurrencyCode = "SAR";
                    vatdata.Id = input.VATDetails.FirstOrDefault()?.Id;
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
                await _invoiceItemsAppService.CreateOrEdit(item);


            }
            var response = new InvoiceResponse();
            try
            {
                response.PdfFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".pdf";
                response.XmlFileUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".xml";
                response.QRCodeUrl = data.UniqueIdentifier + "_" + invoiceno.ToString() + ".png";




                var pathToSave = string.Empty;
                if (AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "")
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/" + AbpSession.TenantId, data.UniqueIdentifier.ToString());
                else
                    pathToSave = Path.Combine("wwwroot/InvoiceFiles/0/", data.UniqueIdentifier.ToString());
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);



                pdfResponse = await _pdfReportAppService.GeneratePdfRequest(input, invoiceno, data.UniqueIdentifier.ToString(), AbpSession.TenantId.ToString(), EInvoicing.Dto.InvoiceTypeEnum.Sales, true);

                int tenantId = AbpSession.TenantId != null && AbpSession.TenantId.ToString() != "" ? (int)AbpSession.TenantId : 0;
                _ = Task.Run(() => AzureUpload.CopyFolderToAzureStorage(pathToSave, tenantId, data.UniqueIdentifier.ToString()));

                //var emaildata = mapper.Map<InvoiceRequest>(input);
                //var email = tenantConfigurationAppService.GetTenantConfigurationByTransactionType("General");

                //if (email.Result?.TenantConfiguration?.EmailJson != null || email.Result?.TenantConfiguration?.EmailJson != "{}")
                //{
                //    var emailsetting = (EmailDto)JsonConvert.DeserializeObject(email.Result.TenantConfiguration.EmailJson, typeof(EmailDto));
                //    _ = Task.Run(() => EmailSender.SendEmail(pdfResponse.PdfFileUrl, emaildata, emailsetting));
                //}


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
                XmlFileUrl = null,
                QRCodeUrl = null
            };

        }


        public async Task<CreateOrEditDraftDto> GetDraftInvoice(int? irnNo, int tenantId, string transType)
       {
            bool isPhase1 = true;

            DataTable dt = new DataTable();
            dt = await _tenantbasicdetails.GetTenantById(tenantId);

            InvoiceResponse pdfResponse = new();
            IRNMaster irn = new();
            CreateOrEditDraftDto data = new();
            CreateOrEditDraftPartyDto supplier = new();



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
                        cmd.CommandText = "GetDraftInvoiceData";
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
                                    data = dataReader.MapToObject<CreateOrEditDraftDto>();
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Items == null)
                                        data.Items = new();
                                    var items = dataReader.MapToObject<CreateOrEditDraftItemDto>();
                                    data.Items.Add(items);
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.InvoiceSummary == null)
                                        data.InvoiceSummary = new();
                                    data.InvoiceSummary = dataReader.MapToObject<CreateOrEditDraftSummaryDto>();
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
                                    var party = dataReader.MapToObject<CreateOrEditDraftPartyDto>();
                                    if (party.Type == "Buyer")
                                    {
                                        data.Buyer.Add(party);
                                    }
                                    if (party.Type == "Delivery")
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
                                    var party = dataReader.MapToObject<CreateOrEditDraftAddressDto>();
                                    if (party.Type == "Buyer")
                                    {
                                        data.Buyer[0].Address = party;
                                    }
                                    if (party.Type == "Delivery")
                                    {
                                        data.Buyer[1].Address = party;
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
                                    var party = dataReader.MapToObject<CreateOrEditDraftContactPersonDto>();
                                    if (party.Type == "Buyer")
                                    {
                                        data.Buyer[0].ContactPerson = party;
                                    }
                                    if (party.Type == "Delivery")
                                    {
                                        data.Buyer[1].ContactPerson = party;
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
                                    var items = dataReader.MapToObject<CreateOrEditDraftVATDetailDto>();
                                    data.VATDetails.Add(items);
                                }
                            }

                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.Discount == null)
                                        data.Discount = new();
                                    var items = dataReader.MapToObject<CreateOrEditDraftDiscountDto>();
                                    data.Discount.Add(items);
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    if (data.PaymentDetails == null)
                                        data.PaymentDetails = new();
                                    var items = dataReader.MapToObject<CreateOrEditDraftPaymentDetailDto>();
                                    data.PaymentDetails.Add(items);
                                }
                            }
                            return data;
                        }

                    }
                    //  return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}