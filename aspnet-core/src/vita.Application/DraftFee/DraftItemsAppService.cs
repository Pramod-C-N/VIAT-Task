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
    [AbpAuthorize(AppPermissions.Pages_DraftItems)]
    public class DraftItemsAppService : vitaAppServiceBase, IDraftItemsAppService
    {
        private readonly IRepository<DraftItem, long> _draftItemRepository;

        public DraftItemsAppService(IRepository<DraftItem, long> draftItemRepository)
        {
            _draftItemRepository = draftItemRepository;

        }

        public virtual async Task<PagedResultDto<GetDraftItemForViewDto>> GetAll(GetAllDraftItemsInput input)
        {

            var filteredDraftItems = _draftItemRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IRNNo.Contains(input.Filter) || e.Identifier.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.BuyerIdentifier.Contains(input.Filter) || e.SellerIdentifier.Contains(input.Filter) || e.StandardIdentifier.Contains(input.Filter) || e.UOM.Contains(input.Filter) || e.VATCode.Contains(input.Filter) || e.CurrencyCode.Contains(input.Filter) || e.TaxSchemeId.Contains(input.Filter) || e.Notes.Contains(input.Filter) || e.ExcemptionReasonCode.Contains(input.Filter) || e.ExcemptionReasonText.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter) || e.AdditionalData2.Contains(input.Filter) || e.Language.Contains(input.Filter))
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExcemptionReasonTextFilter), e => e.ExcemptionReasonText.Contains(input.ExcemptionReasonTextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdditionalData2Filter), e => e.AdditionalData2.Contains(input.AdditionalData2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageFilter), e => e.Language.Contains(input.LanguageFilter));

            var pagedAndFilteredDraftItems = filteredDraftItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var draftItems = from o in pagedAndFilteredDraftItems
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
                                 o.AdditionalData2,
                                 o.Language,
                                 Id = o.Id
                             };

            var totalCount = await filteredDraftItems.CountAsync();

            var dbList = await draftItems.ToListAsync();
            var results = new List<GetDraftItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDraftItemForViewDto()
                {
                    DraftItem = new DraftItemDto
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
                        AdditionalData2 = o.AdditionalData2,
                        Language = o.Language,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDraftItemForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDraftItemForViewDto> GetDraftItemForView(long id)
        {
            var draftItem = await _draftItemRepository.GetAsync(id);

            var output = new GetDraftItemForViewDto { DraftItem = ObjectMapper.Map<DraftItemDto>(draftItem) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DraftItems_Edit)]
        public virtual async Task<GetDraftItemForEditOutput> GetDraftItemForEdit(EntityDto<long> input)
        {
            var draftItem = await _draftItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDraftItemForEditOutput { DraftItem = ObjectMapper.Map<CreateOrEditDraftItemDto>(draftItem) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDraftItemDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DraftItems_Create)]
        protected virtual async Task Create(CreateOrEditDraftItemDto input)
        {
            var draftItem = ObjectMapper.Map<DraftItem>(input);
            draftItem.UniqueIdentifier = Guid.NewGuid();

            if (AbpSession.TenantId != null)
            {
                draftItem.TenantId = (int?)AbpSession.TenantId;
            }

            await _draftItemRepository.InsertAsync(draftItem);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftItems_Edit)]
        protected virtual async Task Update(CreateOrEditDraftItemDto input)
        {
            var draftItem = await _draftItemRepository.FirstOrDefaultAsync((long)input.Id);
            draftItem.UniqueIdentifier = Guid.NewGuid();

            ObjectMapper.Map(input, draftItem);

        }

        [AbpAuthorize(AppPermissions.Pages_DraftItems_Delete)]
        public virtual async Task Delete(EntityDto<long> input)
        {
            await _draftItemRepository.DeleteAsync(input.Id);
        }

    }
}