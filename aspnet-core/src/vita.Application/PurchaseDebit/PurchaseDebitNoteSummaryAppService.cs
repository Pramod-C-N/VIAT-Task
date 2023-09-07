using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.PurchaseDebit.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.PurchaseDebit
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteSummary)]
    public class PurchaseDebitNoteSummaryAppService : vitaAppServiceBase, IPurchaseDebitNoteSummaryAppService
    {
        private readonly IRepository<PurchaseDebitNoteSummary, long> _purchaseDebitNoteSummaryRepository;

        public PurchaseDebitNoteSummaryAppService(IRepository<PurchaseDebitNoteSummary, long> purchaseDebitNoteSummaryRepository)
        {
            _purchaseDebitNoteSummaryRepository = purchaseDebitNoteSummaryRepository;

        }

        public async Task<PagedResultDto<GetPurchaseDebitNoteSummaryForViewDto>> GetAll(GetAllPurchaseDebitNoteSummaryInput input)
        {

            var filteredPurchaseDebitNoteSummary = _purchaseDebitNoteSummaryRepository.GetAll()
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

            var pagedAndFilteredPurchaseDebitNoteSummary = filteredPurchaseDebitNoteSummary
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseDebitNoteSummary = from o in pagedAndFilteredPurchaseDebitNoteSummary
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

            var totalCount = await filteredPurchaseDebitNoteSummary.CountAsync();

            var dbList = await purchaseDebitNoteSummary.ToListAsync();
            var results = new List<GetPurchaseDebitNoteSummaryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseDebitNoteSummaryForViewDto()
                {
                    PurchaseDebitNoteSummary = new PurchaseDebitNoteSummaryDto
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

            return new PagedResultDto<GetPurchaseDebitNoteSummaryForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteSummary_Edit)]
        public async Task<GetPurchaseDebitNoteSummaryForEditOutput> GetPurchaseDebitNoteSummaryForEdit(EntityDto<long> input)
        {
            var purchaseDebitNoteSummary = await _purchaseDebitNoteSummaryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseDebitNoteSummaryForEditOutput { PurchaseDebitNoteSummary = ObjectMapper.Map<CreateOrEditPurchaseDebitNoteSummaryDto>(purchaseDebitNoteSummary) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseDebitNoteSummaryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteSummary_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseDebitNoteSummaryDto input)
        {
            var purchaseDebitNoteSummary = ObjectMapper.Map<PurchaseDebitNoteSummary>(input);
            purchaseDebitNoteSummary.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                purchaseDebitNoteSummary.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseDebitNoteSummaryRepository.InsertAsync(purchaseDebitNoteSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteSummary_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseDebitNoteSummaryDto input)
        {
            var purchaseDebitNoteSummary = await _purchaseDebitNoteSummaryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseDebitNoteSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseDebitNoteSummary_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseDebitNoteSummaryRepository.DeleteAsync(input.Id);
        }

    }
}