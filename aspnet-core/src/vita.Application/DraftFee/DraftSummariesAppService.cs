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
using IdentityServer4.Models;

namespace vita.DraftFee
{
    [AbpAuthorize(AppPermissions.Pages_DraftSummaries)]
    public class DraftSummariesAppService : vitaAppServiceBase, IDraftSummariesAppService
    {
        private readonly IRepository<DraftSummary, long> _draftSummaryRepository;

        public DraftSummariesAppService(IRepository<DraftSummary, long> draftSummaryRepository)
        {
            _draftSummaryRepository = draftSummaryRepository;

        }

        public virtual async Task<PagedResultDto<GetDraftSummaryForViewDto>> GetAll(GetAllDraftSummariesInput input)
        {

            var filteredDraftSummaries = _draftSummaryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.NetInvoiceAmountCurrency.Contains(input.Filter) || e.SumOfInvoiceLineNetAmountCurrency.Contains(input.Filter) || e.TotalAmountWithoutVATCurrency.Contains(input.Filter) || e.CurrencyCode.Contains(input.Filter) || e.PaidAmountCurrency.Contains(input.Filter) || e.PayableAmountCurrency.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(input.MinNetInvoiceAmountFilter != null, e => e.NetInvoiceAmount >= input.MinNetInvoiceAmountFilter)
                        .WhereIf(input.MaxNetInvoiceAmountFilter != null, e => e.NetInvoiceAmount <= input.MaxNetInvoiceAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NetInvoiceAmountCurrencyFilter), e => e.NetInvoiceAmountCurrency.Contains(input.NetInvoiceAmountCurrencyFilter))
                        .WhereIf(input.MinSumOfInvoiceLineNetAmountFilter != null, e => e.SumOfInvoiceLineNetAmount >= input.MinSumOfInvoiceLineNetAmountFilter)
                        .WhereIf(input.MaxSumOfInvoiceLineNetAmountFilter != null, e => e.SumOfInvoiceLineNetAmount <= input.MaxSumOfInvoiceLineNetAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SumOfInvoiceLineNetAmountCurrencyFilter), e => e.SumOfInvoiceLineNetAmountCurrency.Contains(input.SumOfInvoiceLineNetAmountCurrencyFilter))
                        .WhereIf(input.MinTotalAmountWithoutVATFilter != null, e => e.TotalAmountWithoutVAT >= input.MinTotalAmountWithoutVATFilter)
                        .WhereIf(input.MaxTotalAmountWithoutVATFilter != null, e => e.TotalAmountWithoutVAT <= input.MaxTotalAmountWithoutVATFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TotalAmountWithoutVATCurrencyFilter), e => e.TotalAmountWithoutVATCurrency.Contains(input.TotalAmountWithoutVATCurrencyFilter))
                        .WhereIf(input.MinTotalVATAmountFilter != null, e => e.TotalVATAmount >= input.MinTotalVATAmountFilter)
                        .WhereIf(input.MaxTotalVATAmountFilter != null, e => e.TotalVATAmount <= input.MaxTotalVATAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeFilter), e => e.CurrencyCode.Contains(input.CurrencyCodeFilter))
                        .WhereIf(input.MinTotalAmountWithVATFilter != null, e => e.TotalAmountWithVAT >= input.MinTotalAmountWithVATFilter)
                        .WhereIf(input.MaxTotalAmountWithVATFilter != null, e => e.TotalAmountWithVAT <= input.MaxTotalAmountWithVATFilter)
                        .WhereIf(input.MinPaidAmountFilter != null, e => e.PaidAmount >= input.MinPaidAmountFilter)
                        .WhereIf(input.MaxPaidAmountFilter != null, e => e.PaidAmount <= input.MaxPaidAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaidAmountCurrencyFilter), e => e.PaidAmountCurrency.Contains(input.PaidAmountCurrencyFilter))
                        .WhereIf(input.MinPayableAmountFilter != null, e => e.PayableAmount >= input.MinPayableAmountFilter)
                        .WhereIf(input.MaxPayableAmountFilter != null, e => e.PayableAmount <= input.MaxPayableAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayableAmountCurrencyFilter), e => e.PayableAmountCurrency.Contains(input.PayableAmountCurrencyFilter))
                        .WhereIf(input.MinAdvanceAmountwithoutVatFilter != null, e => e.AdvanceAmountwithoutVat >= input.MinAdvanceAmountwithoutVatFilter)
                        .WhereIf(input.MaxAdvanceAmountwithoutVatFilter != null, e => e.AdvanceAmountwithoutVat <= input.MaxAdvanceAmountwithoutVatFilter)
                        .WhereIf(input.MinAdvanceVatFilter != null, e => e.AdvanceVat >= input.MinAdvanceVatFilter)
                        .WhereIf(input.MaxAdvanceVatFilter != null, e => e.AdvanceVat <= input.MaxAdvanceVatFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData1Filter), e => e.AdditionalData1.Contains(input.AdditionalData1Filter));

            var pagedAndFilteredDraftSummaries = filteredDraftSummaries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var draftSummaries = from o in pagedAndFilteredDraftSummaries
                                 select new
                                 {

                                     o.IRNNo,
                                     o.NetInvoiceAmount,
                                     o.NetInvoiceAmountCurrency,
                                     o.SumOfInvoiceLineNetAmount,
                                     o.SumOfInvoiceLineNetAmountCurrency,
                                     o.TotalAmountWithoutVAT,
                                     o.TotalAmountWithoutVATCurrency,
                                     o.TotalVATAmount,
                                     o.CurrencyCode,
                                     o.TotalAmountWithVAT,
                                     o.PaidAmount,
                                     o.PaidAmountCurrency,
                                     o.PayableAmount,
                                     o.PayableAmountCurrency,
                                     o.AdvanceAmountwithoutVat,
                                     o.AdvanceVat,
                                     o.AdditionalData1,
                                     Id = o.Id
                                 };

            var totalCount = await filteredDraftSummaries.CountAsync();

            var dbList = await draftSummaries.ToListAsync();
            var results = new List<GetDraftSummaryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftSummaryForViewDto()
                {
                    DraftSummary = new DraftSummaryDto
                    {

                        IRNNo = o.IRNNo,
                        NetInvoiceAmount = o.NetInvoiceAmount,
                        NetInvoiceAmountCurrency = o.NetInvoiceAmountCurrency,
                        SumOfInvoiceLineNetAmount = o.SumOfInvoiceLineNetAmount,
                        SumOfInvoiceLineNetAmountCurrency = o.SumOfInvoiceLineNetAmountCurrency,
                        TotalAmountWithoutVAT = o.TotalAmountWithoutVAT,
                        TotalAmountWithoutVATCurrency = o.TotalAmountWithoutVATCurrency,
                        TotalVATAmount = o.TotalVATAmount,
                        CurrencyCode = o.CurrencyCode,
                        TotalAmountWithVAT = o.TotalAmountWithVAT,
                        PaidAmount = o.PaidAmount,
                        PaidAmountCurrency = o.PaidAmountCurrency,
                        PayableAmount = o.PayableAmount,
                        PayableAmountCurrency = o.PayableAmountCurrency,
                        AdvanceAmountwithoutVat = o.AdvanceAmountwithoutVat,
                        AdvanceVat = o.AdvanceVat,
                        AdditionalData1 = o.AdditionalData1,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftSummaryForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDraftSummaryForViewDto> GetDraftSummaryForView(long id)
        {
            var draftSummary = await _draftSummaryRepository.GetAsync(id);

            var output = new GetDraftSummaryForViewDto { DraftSummary = ObjectMapper.Map<DraftSummaryDto>(draftSummary) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DraftSummaries_Edit)]
        public virtual async Task<GetDraftSummaryForEditOutput> GetDraftSummaryForEdit(EntityDto<long> input)
        {
            var draftSummary = await _draftSummaryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftSummaryForEditOutput { DraftSummary = ObjectMapper.Map<CreateOrEditDraftSummaryDto>(draftSummary) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDraftSummaryDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DraftSummaries_Create)]
        protected virtual async Task Create(CreateOrEditDraftSummaryDto input)
        {
            var draftSummary = ObjectMapper.Map<DraftSummary>(input);
            draftSummary.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                draftSummary.TenantId = (int?)AbpSession.TenantId;
            }

            await _draftSummaryRepository.InsertAsync(draftSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftSummaries_Edit)]
        protected virtual async Task Update(CreateOrEditDraftSummaryDto input)
        {
            var draftSummary = await _draftSummaryRepository.FirstOrDefaultAsync((long)input.Id);
            draftSummary.UniqueIdentifier = Guid.NewGuid();

            ObjectMapper.Map(input, draftSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftSummaries_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftSummaryRepository.DeleteAsync(input.Id);
        }

    }
}