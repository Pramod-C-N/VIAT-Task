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
    [AbpAuthorize(AppPermissions.Pages_TenantSupplyVATCategory)]
    public class TenantSupplyVATCategoryAppService : vitaAppServiceBase, ITenantSupplyVATCategoryAppService
    {
        private readonly IRepository<TenantSupplyVATCategory> _tenantSupplyVATCategoryRepository;

        public TenantSupplyVATCategoryAppService(IRepository<TenantSupplyVATCategory> tenantSupplyVATCategoryRepository)
        {
            _tenantSupplyVATCategoryRepository = tenantSupplyVATCategoryRepository;

        }

        public async Task<PagedResultDto<GetTenantSupplyVATCategoryForViewDto>> GetAll(GetAllTenantSupplyVATCategoryInput input)
        {

            var filteredTenantSupplyVATCategory = _tenantSupplyVATCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VATCategoryID.Contains(input.Filter) || e.VATCategoryName.Contains(input.Filter) || e.VATCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCategoryIDFilter), e => e.VATCategoryID.Contains(input.VATCategoryIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCategoryNameFilter), e => e.VATCategoryName.Contains(input.VATCategoryNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VATCodeFilter), e => e.VATCode.Contains(input.VATCodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredTenantSupplyVATCategory = filteredTenantSupplyVATCategory
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantSupplyVATCategory = from o in pagedAndFilteredTenantSupplyVATCategory
                                          select new
                                          {

                                              o.VATCategoryID,
                                              o.VATCategoryName,
                                              o.VATCode,
                                              o.IsActive,
                                              Id = o.Id
                                          };

            var totalCount = await filteredTenantSupplyVATCategory.CountAsync();

            var dbList = await tenantSupplyVATCategory.ToListAsync();
            var results = new List<GetTenantSupplyVATCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantSupplyVATCategoryForViewDto()
                {
                    TenantSupplyVATCategory = new TenantSupplyVATCategoryDto
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

            return new PagedResultDto<GetTenantSupplyVATCategoryForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TenantSupplyVATCategory_Edit)]
        public async Task<GetTenantSupplyVATCategoryForEditOutput> GetTenantSupplyVATCategoryForEdit(EntityDto input)
        {
            var tenantSupplyVATCategory = await _tenantSupplyVATCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantSupplyVATCategoryForEditOutput { TenantSupplyVATCategory = ObjectMapper.Map<CreateOrEditTenantSupplyVATCategoryDto>(tenantSupplyVATCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantSupplyVATCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantSupplyVATCategory_Create)]
        protected virtual async Task Create(CreateOrEditTenantSupplyVATCategoryDto input)
        {
            var tenantSupplyVATCategory = ObjectMapper.Map<TenantSupplyVATCategory>(input);

            if (AbpSession.TenantId != null)
            {
                tenantSupplyVATCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantSupplyVATCategoryRepository.InsertAsync(tenantSupplyVATCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantSupplyVATCategory_Edit)]
        protected virtual async Task Update(CreateOrEditTenantSupplyVATCategoryDto input)
        {
            var tenantSupplyVATCategory = await _tenantSupplyVATCategoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantSupplyVATCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantSupplyVATCategory_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantSupplyVATCategoryRepository.DeleteAsync(input.Id);
        }

    }
}