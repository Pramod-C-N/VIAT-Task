using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.TenantDetails.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.TenantDetails
{
    [AbpAuthorize(AppPermissions.Pages_TenantPurchaseVatCateory)]
    public class TenantPurchaseVatCateoryAppService : vitaAppServiceBase, ITenantPurchaseVatCateoryAppService
    {
        private readonly IRepository<TenantPurchaseVatCateory> _tenantPurchaseVatCateoryRepository;

        public TenantPurchaseVatCateoryAppService(IRepository<TenantPurchaseVatCateory> tenantPurchaseVatCateoryRepository)
        {
            _tenantPurchaseVatCateoryRepository = tenantPurchaseVatCateoryRepository;

        }

        public async Task<PagedResultDto<GetTenantPurchaseVatCateoryForViewDto>> GetAll(GetAllTenantPurchaseVatCateoryInput input)
        {

            var filteredTenantPurchaseVatCateory = _tenantPurchaseVatCateoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VATCategoryID.Contains(input.Filter) || e.VATCategoryName.Contains(input.Filter) || e.VATCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCategoryIDFilter), e => e.VATCategoryID.Contains(input.VATCategoryIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCategoryNameFilter), e => e.VATCategoryName.Contains(input.VATCategoryNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCodeFilter), e => e.VATCode.Contains(input.VATCodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredTenantPurchaseVatCateory = filteredTenantPurchaseVatCateory
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantPurchaseVatCateory = from o in pagedAndFilteredTenantPurchaseVatCateory
                                           select new
                                           {

                                               o.VATCategoryID,
                                               o.VATCategoryName,
                                               o.VATCode,
                                               o.IsActive,
                                               Id = o.Id
                                           };

            var totalCount = await filteredTenantPurchaseVatCateory.CountAsync();

            var dbList = await tenantPurchaseVatCateory.ToListAsync();
            var results = new List<GetTenantPurchaseVatCateoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantPurchaseVatCateoryForViewDto()
                {
                    TenantPurchaseVatCateory = new TenantPurchaseVatCateoryDto
                    {

                        VATCategoryID = o.VATCategoryID,
                        VATCategoryName = o.VATCategoryName,
                        VATCode = o.VATCode,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantPurchaseVatCateoryForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TenantPurchaseVatCateory_Edit)]
        public async Task<GetTenantPurchaseVatCateoryForEditOutput> GetTenantPurchaseVatCateoryForEdit(EntityDto input)
        {
            var tenantPurchaseVatCateory = await _tenantPurchaseVatCateoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantPurchaseVatCateoryForEditOutput { TenantPurchaseVatCateory = ObjectMapper.Map<CreateOrEditTenantPurchaseVatCateoryDto>(tenantPurchaseVatCateory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantPurchaseVatCateoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantPurchaseVatCateory_Create)]
        protected virtual async Task Create(CreateOrEditTenantPurchaseVatCateoryDto input)
        {
            var tenantPurchaseVatCateory = ObjectMapper.Map<TenantPurchaseVatCateory>(input);

            if (AbpSession.TenantId != null)
            {
                tenantPurchaseVatCateory.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantPurchaseVatCateoryRepository.InsertAsync(tenantPurchaseVatCateory);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantPurchaseVatCateory_Edit)]
        protected virtual async Task Update(CreateOrEditTenantPurchaseVatCateoryDto input)
        {
            var tenantPurchaseVatCateory = await _tenantPurchaseVatCateoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantPurchaseVatCateory);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantPurchaseVatCateory_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantPurchaseVatCateoryRepository.DeleteAsync(input.Id);
        }

    }
}