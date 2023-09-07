using System;
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

namespace vita.Debit
{
    [AbpAuthorize(AppPermissions.Pages_DebitNoteSummaries)]
    public class DebitNoteSummariesAppService : vitaAppServiceBase, IDebitNoteSummariesAppService
    {
        private readonly IRepository<DebitNoteSummary, long> _debitNoteSummaryRepository;

        public DebitNoteSummariesAppService(IRepository<DebitNoteSummary, long> debitNoteSummaryRepository)
        {
            _debitNoteSummaryRepository = debitNoteSummaryRepository;

        }

        public async Task<PagedResultDto<GetDebitNoteSummaryForViewDto>> GetAll(GetAllDebitNoteSummariesInput input)
        {

            var filteredDebitNoteSummaries = _debitNoteSummaryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.NetInvoiceAmountCurrency.Contains(input.Filter) || e.SumOfInvoiceLineNetAmountCurrency.Contains(input.Filter) || e.TotalAmountWithoutVATCurrency.Contains(input.Filter) || e.CurrencyCode.Contains(input.Filter) || e.PaidAmountCurrency.Contains(input.Filter) || e.PayableAmountCurrency.Contains(input.Filter))
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
                        .WhereIf(input.MaxAdvanceVatFilter != null, e => e.AdvanceVat <= input.MaxAdvanceVatFilter).WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData1Filter), e => e.AdditionalData1.Contains(input.AdditionalData1Filter));

            var pagedAndFilteredDebitNoteSummaries = filteredDebitNoteSummaries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNoteSummaries = from o in pagedAndFilteredDebitNoteSummaries
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

            var totalCount = await filteredDebitNoteSummaries.CountAsync();

            var dbList = await debitNoteSummaries.ToListAsync();
            var results = new List<GetDebitNoteSummaryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNoteSummaryForViewDto()
                {
                    DebitNoteSummary = new DebitNoteSummaryDto
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
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDebitNoteSummaryForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteSummaries_Edit)]
        public async Task<GetDebitNoteSummaryForEditOutput> GetDebitNoteSummaryForEdit(EntityDto<long> input)
        {
            var debitNoteSummary = await _debitNoteSummaryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDebitNoteSummaryForEditOutput { DebitNoteSummary = ObjectMapper.Map<CreateOrEditDebitNoteSummaryDto>(debitNoteSummary) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDebitNoteSummaryDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteSummaries_Create)]
        protected virtual async Task Create(CreateOrEditDebitNoteSummaryDto input)
        {
            var debitNoteSummary = ObjectMapper.Map<DebitNoteSummary>(input);
            debitNoteSummary.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                debitNoteSummary.TenantId = (int?)AbpSession.TenantId;
            }

            await _debitNoteSummaryRepository.InsertAsync(debitNoteSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteSummaries_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNoteSummaryDto input)
        {
            var debitNoteSummary = await _debitNoteSummaryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, debitNoteSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteSummaries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _debitNoteSummaryRepository.DeleteAsync(input.Id);
        }

    }
}