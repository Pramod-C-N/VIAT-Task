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
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntryDiscounts)]
    public class PurchaseEntryDiscountsAppService : vitaAppServiceBase, IPurchaseEntryDiscountsAppService
    {
        private readonly IRepository<PurchaseEntryDiscount, long> _purchaseEntryDiscountRepository;

        public PurchaseEntryDiscountsAppService(IRepository<PurchaseEntryDiscount, long> purchaseEntryDiscountRepository)
        {
            _purchaseEntryDiscountRepository = purchaseEntryDiscountRepository;

        }

        public async Task<PagedResultDto<GetPurchaseEntryDiscountForViewDto>> GetAll(GetAllPurchaseEntryDiscountsInput input)
        {

            var filteredPurchaseEntryDiscounts = _purchaseEntryDiscountRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.VATCode.Contains(input.Filter) || e.TaxSchemeId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCodeFilter), e => e.VATCode.Contains(input.VATCodeFilter))
                        .WhereIf(input.MinVATRateFilter != null, e => e.VATRate >= input.MinVATRateFilter)
                        .WhereIf(input.MaxVATRateFilter != null, e => e.VATRate <= input.MaxVATRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxSchemeIdFilter), e => e.TaxSchemeId.Contains(input.TaxSchemeIdFilter));

            var pagedAndFilteredPurchaseEntryDiscounts = filteredPurchaseEntryDiscounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntryDiscounts = from o in pagedAndFilteredPurchaseEntryDiscounts
                                         select new
                                         {

                                             o.IRNNo,
                                             o.DiscountPercentage,
                                             o.DiscountAmount,
                                             o.VATCode,
                                             o.VATRate,
                                             o.TaxSchemeId,
                                             Id = o.Id
                                         };

            var totalCount = await filteredPurchaseEntryDiscounts.CountAsync();

            var dbList = await purchaseEntryDiscounts.ToListAsync();
            var results = new List<GetPurchaseEntryDiscountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntryDiscountForViewDto()
                {
                    PurchaseEntryDiscount = new PurchaseEntryDiscountDto
                    {

                        IRNNo = o.IRNNo,
                        DiscountPercentage = o.DiscountPercentage,
                        DiscountAmount = o.DiscountAmount,
                        VATCode = o.VATCode,
                        VATRate = o.VATRate,
                        TaxSchemeId = o.TaxSchemeId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPurchaseEntryDiscountForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryDiscounts_Edit)]
        public async Task<GetPurchaseEntryDiscountForEditOutput> GetPurchaseEntryDiscountForEdit(EntityDto<long> input)
        {
            var purchaseEntryDiscount = await _purchaseEntryDiscountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntryDiscountForEditOutput { PurchaseEntryDiscount = ObjectMapper.Map<CreateOrEditPurchaseEntryDiscountDto>(purchaseEntryDiscount) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseEntryDiscountDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryDiscounts_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseEntryDiscountDto input)
        {
            var purchaseEntryDiscount = ObjectMapper.Map<PurchaseEntryDiscount>(input);
            purchaseEntryDiscount.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseEntryDiscount.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseEntryDiscountRepository.InsertAsync(purchaseEntryDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryDiscounts_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntryDiscountDto input)
        {
            var purchaseEntryDiscount = await _purchaseEntryDiscountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntryDiscount);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryDiscounts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntryDiscountRepository.DeleteAsync(input.Id);
        }

    }
}