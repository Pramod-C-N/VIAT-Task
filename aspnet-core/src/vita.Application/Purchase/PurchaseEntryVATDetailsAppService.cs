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
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntryVATDetails)]
    public class PurchaseEntryVATDetailsAppService : vitaAppServiceBase, IPurchaseEntryVATDetailsAppService
    {
        private readonly IRepository<PurchaseEntryVATDetail, long> _purchaseEntryVATDetailRepository;

        public PurchaseEntryVATDetailsAppService(IRepository<PurchaseEntryVATDetail, long> purchaseEntryVATDetailRepository)
        {
            _purchaseEntryVATDetailRepository = purchaseEntryVATDetailRepository;

        }

        public async Task<PagedResultDto<GetPurchaseEntryVATDetailForViewDto>> GetAll(GetAllPurchaseEntryVATDetailsInput input)
        {

            var filteredPurchaseEntryVATDetails = _purchaseEntryVATDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.TaxSchemeId.Contains(input.Filter) || e.VATCode.Contains(input.Filter) || e.ExcemptionReasonCode.Contains(input.Filter) || e.ExcemptionReasonText.Contains(input.Filter) || e.CurrencyCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxSchemeIdFilter), e => e.TaxSchemeId.Contains(input.TaxSchemeIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCodeFilter), e => e.VATCode.Contains(input.VATCodeFilter))
                        .WhereIf(input.MinVATRateFilter != null, e => e.VATRate >= input.MinVATRateFilter)
                        .WhereIf(input.MaxVATRateFilter != null, e => e.VATRate <= input.MaxVATRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExcemptionReasonCodeFilter), e => e.ExcemptionReasonCode.Contains(input.ExcemptionReasonCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExcemptionReasonTextFilter), e => e.ExcemptionReasonText.Contains(input.ExcemptionReasonTextFilter))
                        .WhereIf(input.MinTaxableAmountFilter != null, e => e.TaxableAmount >= input.MinTaxableAmountFilter)
                        .WhereIf(input.MaxTaxableAmountFilter != null, e => e.TaxableAmount <= input.MaxTaxableAmountFilter)
                        .WhereIf(input.MinTaxAmountFilter != null, e => e.TaxAmount >= input.MinTaxAmountFilter)
                        .WhereIf(input.MaxTaxAmountFilter != null, e => e.TaxAmount <= input.MaxTaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeFilter), e => e.CurrencyCode.Contains(input.CurrencyCodeFilter));

            var pagedAndFilteredPurchaseEntryVATDetails = filteredPurchaseEntryVATDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntryVATDetails = from o in pagedAndFilteredPurchaseEntryVATDetails
                                          select new
                                          {

                                              o.IRNNo,
                                              o.TaxSchemeId,
                                              o.VATCode,
                                              o.VATRate,
                                              o.ExcemptionReasonCode,
                                              o.ExcemptionReasonText,
                                              o.TaxableAmount,
                                              o.TaxAmount,
                                              o.CurrencyCode,
                                              Id = o.Id
                                          };

            var totalCount = await filteredPurchaseEntryVATDetails.CountAsync();

            var dbList = await purchaseEntryVATDetails.ToListAsync();
            var results = new List<GetPurchaseEntryVATDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntryVATDetailForViewDto()
                {
                    PurchaseEntryVATDetail = new PurchaseEntryVATDetailDto
                    {

                        IRNNo = o.IRNNo,
                        TaxSchemeId = o.TaxSchemeId,
                        VATCode = o.VATCode,
                        VATRate = o.VATRate,
                        ExcemptionReasonCode = o.ExcemptionReasonCode,
                        ExcemptionReasonText = o.ExcemptionReasonText,
                        TaxableAmount = o.TaxableAmount,
                        TaxAmount = o.TaxAmount,
                        CurrencyCode = o.CurrencyCode,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPurchaseEntryVATDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryVATDetails_Edit)]
        public async Task<GetPurchaseEntryVATDetailForEditOutput> GetPurchaseEntryVATDetailForEdit(EntityDto<long> input)
        {
            var purchaseEntryVATDetail = await _purchaseEntryVATDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntryVATDetailForEditOutput { PurchaseEntryVATDetail = ObjectMapper.Map<CreateOrEditPurchaseEntryVATDetailDto>(purchaseEntryVATDetail) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseEntryVATDetailDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryVATDetails_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseEntryVATDetailDto input)
        {
            var purchaseEntryVATDetail = ObjectMapper.Map<PurchaseEntryVATDetail>(input);
            purchaseEntryVATDetail.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseEntryVATDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseEntryVATDetailRepository.InsertAsync(purchaseEntryVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryVATDetails_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntryVATDetailDto input)
        {
            var purchaseEntryVATDetail = await _purchaseEntryVATDetailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntryVATDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryVATDetails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntryVATDetailRepository.DeleteAsync(input.Id);
        }

    }
}