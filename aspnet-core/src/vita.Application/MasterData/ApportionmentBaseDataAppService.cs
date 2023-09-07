using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.MasterData.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.MasterData
{
    [AbpAuthorize(AppPermissions.Pages_ApportionmentBaseData)]
    public class ApportionmentBaseDataAppService : vitaAppServiceBase, IApportionmentBaseDataAppService
    {
        private readonly IRepository<ApportionmentBaseData> _apportionmentBaseDataRepository;

        public ApportionmentBaseDataAppService(IRepository<ApportionmentBaseData> apportionmentBaseDataRepository)
        {
            _apportionmentBaseDataRepository = apportionmentBaseDataRepository;

        }

        public async Task<PagedResultDto<GetApportionmentBaseDataForViewDto>> GetAll(GetAllApportionmentBaseDataInput input)
        {

            var filteredApportionmentBaseData = _apportionmentBaseDataRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FinYear.Contains(input.Filter))
                        .WhereIf(input.MinEffectiveFromDateFilter != null, e => e.EffectiveFromDate >= input.MinEffectiveFromDateFilter)
                        .WhereIf(input.MaxEffectiveFromDateFilter != null, e => e.EffectiveFromDate <= input.MaxEffectiveFromDateFilter)
                        .WhereIf(input.MinEffectiveTillEndDateFilter != null, e => e.EffectiveTillEndDate >= input.MinEffectiveTillEndDateFilter)
                        .WhereIf(input.MaxEffectiveTillEndDateFilter != null, e => e.EffectiveTillEndDate <= input.MaxEffectiveTillEndDateFilter)
                        .WhereIf(input.MinTaxableSupplyFilter != null, e => e.TaxableSupply >= input.MinTaxableSupplyFilter)
                        .WhereIf(input.MaxTaxableSupplyFilter != null, e => e.TaxableSupply <= input.MaxTaxableSupplyFilter)
                        .WhereIf(input.MinTotalSupplyFilter != null, e => e.TotalSupply >= input.MinTotalSupplyFilter)
                        .WhereIf(input.MaxTotalSupplyFilter != null, e => e.TotalSupply <= input.MaxTotalSupplyFilter)
                        .WhereIf(input.MinTaxablePurchaseFilter != null, e => e.TaxablePurchase >= input.MinTaxablePurchaseFilter)
                        .WhereIf(input.MaxTaxablePurchaseFilter != null, e => e.TaxablePurchase <= input.MaxTaxablePurchaseFilter)
                        .WhereIf(input.MinTotalPurchaseFilter != null, e => e.TotalPurchase >= input.MinTotalPurchaseFilter)
                        .WhereIf(input.MaxTotalPurchaseFilter != null, e => e.TotalPurchase <= input.MaxTotalPurchaseFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FinYearFilter), e => e.FinYear.Contains(input.FinYearFilter))
                        .WhereIf(input.MinTotalExemptSalesFilter != null, e => e.TotalExemptSales >= input.MinTotalExemptSalesFilter)
                        .WhereIf(input.MaxTotalExemptSalesFilter != null, e => e.TotalExemptSales <= input.MaxTotalExemptSalesFilter)
                        .WhereIf(input.MinTotalExemptPurchaseFilter != null, e => e.TotalExemptPurchase >= input.MinTotalExemptPurchaseFilter)
                        .WhereIf(input.MaxTotalExemptPurchaseFilter != null, e => e.TotalExemptPurchase <= input.MaxTotalExemptPurchaseFilter)
                        .WhereIf(input.MinMixedOverHeadsFilter != null, e => e.MixedOverHeads >= input.MinMixedOverHeadsFilter)
                        .WhereIf(input.MaxMixedOverHeadsFilter != null, e => e.MixedOverHeads <= input.MaxMixedOverHeadsFilter)
                        .WhereIf(input.MinApportionmentSuppliesFilter != null, e => e.ApportionmentSupplies >= input.MinApportionmentSuppliesFilter)
                        .WhereIf(input.MaxApportionmentSuppliesFilter != null, e => e.ApportionmentSupplies <= input.MaxApportionmentSuppliesFilter)
                        .WhereIf(input.MinApportionmentPurchasesFilter != null, e => e.ApportionmentPurchases >= input.MinApportionmentPurchasesFilter)
                        .WhereIf(input.MaxApportionmentPurchasesFilter != null, e => e.ApportionmentPurchases <= input.MaxApportionmentPurchasesFilter);

            var pagedAndFilteredApportionmentBaseData = filteredApportionmentBaseData
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var apportionmentBaseData = from o in pagedAndFilteredApportionmentBaseData
                                        select new
                                        {

                                            o.EffectiveFromDate,
                                            o.EffectiveTillEndDate,
                                            o.TaxableSupply,
                                            o.TotalSupply,
                                            o.TaxablePurchase,
                                            o.TotalPurchase,
                                            o.FinYear,
                                            o.TotalExemptSales,
                                            o.TotalExemptPurchase,
                                            o.MixedOverHeads,
                                            o.ApportionmentSupplies,
                                            o.ApportionmentPurchases,
                                            Id = o.Id
                                        };

            var totalCount = await filteredApportionmentBaseData.CountAsync();

            var dbList = await apportionmentBaseData.ToListAsync();
            var results = new List<GetApportionmentBaseDataForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetApportionmentBaseDataForViewDto()
                {
                    ApportionmentBaseData = new ApportionmentBaseDataDto
                    {

                        EffectiveFromDate = o.EffectiveFromDate,
                        EffectiveTillEndDate = o.EffectiveTillEndDate,
                        TaxableSupply = o.TaxableSupply,
                        TotalSupply = o.TotalSupply,
                        TaxablePurchase = o.TaxablePurchase,
                        TotalPurchase = o.TotalPurchase,
                        FinYear = o.FinYear,
                        TotalExemptSales = o.TotalExemptSales,
                        TotalExemptPurchase = o.TotalExemptPurchase,
                        MixedOverHeads = o.MixedOverHeads,
                        ApportionmentSupplies = o.ApportionmentSupplies,
                        ApportionmentPurchases = o.ApportionmentPurchases,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetApportionmentBaseDataForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetApportionmentBaseDataForViewDto> GetApportionmentBaseDataForView(int id)
        {
            var apportionmentBaseData = await _apportionmentBaseDataRepository.GetAsync(id);

            var output = new GetApportionmentBaseDataForViewDto { ApportionmentBaseData = ObjectMapper.Map<ApportionmentBaseDataDto>(apportionmentBaseData) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ApportionmentBaseData_Edit)]
        public async Task<GetApportionmentBaseDataForEditOutput> GetApportionmentBaseDataForEdit(EntityDto input)
        {
            var apportionmentBaseData = await _apportionmentBaseDataRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApportionmentBaseDataForEditOutput { ApportionmentBaseData = ObjectMapper.Map<CreateOrEditApportionmentBaseDataDto>(apportionmentBaseData) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditApportionmentBaseDataDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ApportionmentBaseData_Create)]
        protected virtual async Task Create(CreateOrEditApportionmentBaseDataDto input)
        {
            var apportionmentBaseData = ObjectMapper.Map<ApportionmentBaseData>(input);

            if (AbpSession.TenantId != null)
            {
                apportionmentBaseData.TenantId = (int?)AbpSession.TenantId;
            }

            await _apportionmentBaseDataRepository.InsertAsync(apportionmentBaseData);

        }

        [AbpAuthorize(AppPermissions.Pages_ApportionmentBaseData_Edit)]
        protected virtual async Task Update(CreateOrEditApportionmentBaseDataDto input)
        {
            var apportionmentBaseData = await _apportionmentBaseDataRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, apportionmentBaseData);

        }

        [AbpAuthorize(AppPermissions.Pages_ApportionmentBaseData_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _apportionmentBaseDataRepository.DeleteAsync(input.Id);
        }

    }
}