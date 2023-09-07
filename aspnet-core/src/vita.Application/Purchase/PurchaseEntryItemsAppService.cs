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
    [AbpAuthorize(AppPermissions.Pages_PurchaseEntryItems)]
    public class PurchaseEntryItemsAppService : vitaAppServiceBase, IPurchaseEntryItemsAppService
    {
        private readonly IRepository<PurchaseEntryItem, long> _purchaseEntryItemRepository;

        public PurchaseEntryItemsAppService(IRepository<PurchaseEntryItem, long> purchaseEntryItemRepository)
        {
            _purchaseEntryItemRepository = purchaseEntryItemRepository;

        }

        public async Task<PagedResultDto<GetPurchaseEntryItemForViewDto>> GetAll(GetAllPurchaseEntryItemsInput input)
        {

            var filteredPurchaseEntryItems = _purchaseEntryItemRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.Identifier.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.BuyerIdentifier.Contains(input.Filter) || e.SellerIdentifier.Contains(input.Filter) || e.StandardIdentifier.Contains(input.Filter) || e.UOM.Contains(input.Filter) || e.VATCode.Contains(input.Filter) || e.CurrencyCode.Contains(input.Filter) || e.TaxSchemeId.Contains(input.Filter) || e.Notes.Contains(input.Filter) || e.ExcemptionReasonCode.Contains(input.Filter) || e.ExcemptionReasonText.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IRNNoFilter), e => e.IRNNo.Contains(input.IRNNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IdentifierFilter), e => e.Identifier.Contains(input.IdentifierFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BuyerIdentifierFilter), e => e.BuyerIdentifier.Contains(input.BuyerIdentifierFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SellerIdentifierFilter), e => e.SellerIdentifier.Contains(input.SellerIdentifierFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StandardIdentifierFilter), e => e.StandardIdentifier.Contains(input.StandardIdentifierFilter))
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UOMFilter), e => e.UOM.Contains(input.UOMFilter))
                        .WhereIf(input.MinUnitPriceFilter != null, e => e.UnitPrice >= input.MinUnitPriceFilter)
                        .WhereIf(input.MaxUnitPriceFilter != null, e => e.UnitPrice <= input.MaxUnitPriceFilter)
                        .WhereIf(input.MinCostPriceFilter != null, e => e.CostPrice >= input.MinCostPriceFilter)
                        .WhereIf(input.MaxCostPriceFilter != null, e => e.CostPrice <= input.MaxCostPriceFilter)
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinGrossPriceFilter != null, e => e.GrossPrice >= input.MinGrossPriceFilter)
                        .WhereIf(input.MaxGrossPriceFilter != null, e => e.GrossPrice <= input.MaxGrossPriceFilter)
                        .WhereIf(input.MinNetPriceFilter != null, e => e.NetPrice >= input.MinNetPriceFilter)
                        .WhereIf(input.MaxNetPriceFilter != null, e => e.NetPrice <= input.MaxNetPriceFilter)
                        .WhereIf(input.MinVATRateFilter != null, e => e.VATRate >= input.MinVATRateFilter)
                        .WhereIf(input.MaxVATRateFilter != null, e => e.VATRate <= input.MaxVATRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCodeFilter), e => e.VATCode.Contains(input.VATCodeFilter))
                        .WhereIf(input.MinVATAmountFilter != null, e => e.VATAmount >= input.MinVATAmountFilter)
                        .WhereIf(input.MaxVATAmountFilter != null, e => e.VATAmount <= input.MaxVATAmountFilter)
                        .WhereIf(input.MinLineAmountInclusiveVATFilter != null, e => e.LineAmountInclusiveVAT >= input.MinLineAmountInclusiveVATFilter)
                        .WhereIf(input.MaxLineAmountInclusiveVATFilter != null, e => e.LineAmountInclusiveVAT <= input.MaxLineAmountInclusiveVATFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeFilter), e => e.CurrencyCode.Contains(input.CurrencyCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxSchemeIdFilter), e => e.TaxSchemeId.Contains(input.TaxSchemeIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExcemptionReasonCodeFilter), e => e.ExcemptionReasonCode.Contains(input.ExcemptionReasonCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExcemptionReasonTextFilter), e => e.ExcemptionReasonText.Contains(input.ExcemptionReasonTextFilter));

            var pagedAndFilteredPurchaseEntryItems = filteredPurchaseEntryItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseEntryItems = from o in pagedAndFilteredPurchaseEntryItems
                                     select new
                                     {

                                         o.IRNNo,
                                         o.Identifier,
                                         o.Name,
                                         o.Description,
                                         o.BuyerIdentifier,
                                         o.SellerIdentifier,
                                         o.StandardIdentifier,
                                         o.Quantity,
                                         o.UOM,
                                         o.UnitPrice,
                                         o.CostPrice,
                                         o.DiscountPercentage,
                                         o.DiscountAmount,
                                         o.GrossPrice,
                                         o.NetPrice,
                                         o.VATRate,
                                         o.VATCode,
                                         o.VATAmount,
                                         o.LineAmountInclusiveVAT,
                                         o.CurrencyCode,
                                         o.TaxSchemeId,
                                         o.Notes,
                                         o.ExcemptionReasonCode,
                                         o.ExcemptionReasonText,
                                         Id = o.Id
                                     };

            var totalCount = await filteredPurchaseEntryItems.CountAsync();

            var dbList = await purchaseEntryItems.ToListAsync();
            var results = new List<GetPurchaseEntryItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPurchaseEntryItemForViewDto()
                {
                    PurchaseEntryItem = new PurchaseEntryItemDto
                    {

                        IRNNo = o.IRNNo,
                        Identifier = o.Identifier,
                        Name = o.Name,
                        Description = o.Description,
                        BuyerIdentifier = o.BuyerIdentifier,
                        SellerIdentifier = o.SellerIdentifier,
                        StandardIdentifier = o.StandardIdentifier,
                        Quantity = o.Quantity,
                        UOM = o.UOM,
                        UnitPrice = o.UnitPrice,
                        CostPrice = o.CostPrice,
                        DiscountPercentage = o.DiscountPercentage,
                        DiscountAmount = o.DiscountAmount,
                        GrossPrice = o.GrossPrice,
                        NetPrice = o.NetPrice,
                        VATRate = o.VATRate,
                        VATCode = o.VATCode,
                        VATAmount = o.VATAmount,
                        LineAmountInclusiveVAT = o.LineAmountInclusiveVAT,
                        CurrencyCode = o.CurrencyCode,
                        TaxSchemeId = o.TaxSchemeId,
                        Notes = o.Notes,
                        ExcemptionReasonCode = o.ExcemptionReasonCode,
                        ExcemptionReasonText = o.ExcemptionReasonText,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPurchaseEntryItemForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryItems_Edit)]
        public async Task<GetPurchaseEntryItemForEditOutput> GetPurchaseEntryItemForEdit(EntityDto<long> input)
        {
            var purchaseEntryItem = await _purchaseEntryItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseEntryItemForEditOutput { PurchaseEntryItem = ObjectMapper.Map<CreateOrEditPurchaseEntryItemDto>(purchaseEntryItem) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseEntryItemDto input)
        {
            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryItems_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseEntryItemDto input)
        {
            var purchaseEntryItem = ObjectMapper.Map<PurchaseEntryItem>(input);
            purchaseEntryItem.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                purchaseEntryItem.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseEntryItemRepository.InsertAsync(purchaseEntryItem);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryItems_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseEntryItemDto input)
        {
            var purchaseEntryItem = await _purchaseEntryItemRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseEntryItem);

        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseEntryItems_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseEntryItemRepository.DeleteAsync(input.Id);
        }

    }
}