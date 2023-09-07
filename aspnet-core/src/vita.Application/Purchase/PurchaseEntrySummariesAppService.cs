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

namespace vita.Purchase
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntrySummaries)]
    public class PurchaseEntrySummariesAppService : vitaAppServiceBase, IPurchaseEntrySummariesAppService
    {
        private readonly IRepository<PurchaseEntrySummary, long> _purchaseEntrySummaryRepository;

        public PurchaseEntrySummariesAppService(IRepository<PurchaseEntrySummary, long> purchaseEntrySummaryRepository)
        {
            _purchaseEntrySummaryRepository = purchaseEntrySummaryRepository;

        }

        public async Task<PagedResultDto<GetPurchaseEntrySummaryForViewDto>> GetAll(GetAllPurchaseEntrySummariesInput input)
        {

            var filteredPurchaseEntrySummaries = _purchaseEntrySummaryRepository.GetAll()
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
                        .WhereIf(input.MaxAdvanceVatFilter != null, e => e.AdvanceVat <= input.MaxAdvanceVatFilter);

            var pagedAndFilteredPurchaseEntrySummaries = filteredPurchaseEntrySummaries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntrySummaries = from o in pagedAndFilteredPurchaseEntrySummaries
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
                                             Id = o.Id
                                         };

            var totalCount = await filteredPurchaseEntrySummaries.CountAsync();

            var dbList = await purchaseEntrySummaries.ToListAsync();
            var results = new List<GetPurchaseEntrySummaryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntrySummaryForViewDto()
                {
                    PurchaseEntrySummary = new PurchaseEntrySummaryDto
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

            return new PagedResultDto<GetPurchaseEntrySummaryForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntrySummaries_Edit)]
        public async Task<GetPurchaseEntrySummaryForEditOutput> GetPurchaseEntrySummaryForEdit(EntityDto<long> input)
        {
            var purchaseEntrySummary = await _purchaseEntrySummaryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntrySummaryForEditOutput { PurchaseEntrySummary = ObjectMapper.Map<CreateOrEditPurchaseEntrySummaryDto>(purchaseEntrySummary) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseEntrySummaryDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntrySummaries_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseEntrySummaryDto input)
        {
            var purchaseEntrySummary = ObjectMapper.Map<PurchaseEntrySummary>(input);
            purchaseEntrySummary.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseEntrySummary.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseEntrySummaryRepository.InsertAsync(purchaseEntrySummary);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntrySummaries_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntrySummaryDto input)
        {
            var purchaseEntrySummary = await _purchaseEntrySummaryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntrySummary);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntrySummaries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntrySummaryRepository.DeleteAsync(input.Id);
        }

    }
}