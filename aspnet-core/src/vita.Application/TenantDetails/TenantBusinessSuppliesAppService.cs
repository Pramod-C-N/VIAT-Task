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
    [AbpAuthorize(AppPermissions.Pages_TenantBusinessSupplies)]
    public class TenantBusinessSuppliesAppService : vitaAppServiceBase, ITenantBusinessSuppliesAppService
    {
        private readonly IRepository<TenantBusinessSupplies> _tenantBusinessSuppliesRepository;

        public TenantBusinessSuppliesAppService(IRepository<TenantBusinessSupplies> tenantBusinessSuppliesRepository)
        {
            _tenantBusinessSuppliesRepository = tenantBusinessSuppliesRepository;

        }

        public async Task<PagedResultDto<GetTenantBusinessSuppliesForViewDto>> GetAll(GetAllTenantBusinessSuppliesInput input)
        {

            var filteredTenantBusinessSupplies = _tenantBusinessSuppliesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BusinessTypeID.Contains(input.Filter) || e.BusinessSupplies.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessTypeIDFilter), e => e.BusinessTypeID.Contains(input.BusinessTypeIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessSuppliesFilter), e => e.BusinessSupplies.Contains(input.BusinessSuppliesFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredTenantBusinessSupplies = filteredTenantBusinessSupplies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantBusinessSupplies = from o in pagedAndFilteredTenantBusinessSupplies
                                         select new
                                         {

                                             o.BusinessTypeID,
                                             o.BusinessSupplies,
                                             o.IsActive,
                                             Id = o.Id
                                         };

            var totalCount = await filteredTenantBusinessSupplies.CountAsync();

            var dbList = await tenantBusinessSupplies.ToListAsync();
            var results = new List<GetTenantBusinessSuppliesForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantBusinessSuppliesForViewDto()
                {
                    TenantBusinessSupplies = new TenantBusinessSuppliesDto
                    {

                        BusinessTypeID = o.BusinessTypeID,
                        BusinessSupplies = o.BusinessSupplies,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantBusinessSuppliesForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBusinessSupplies_Edit)]
        public async Task<GetTenantBusinessSuppliesForEditOutput> GetTenantBusinessSuppliesForEdit(EntityDto input)
        {
            var tenantBusinessSupplies = await _tenantBusinessSuppliesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantBusinessSuppliesForEditOutput { TenantBusinessSupplies = ObjectMapper.Map<CreateOrEditTenantBusinessSuppliesDto>(tenantBusinessSupplies) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantBusinessSuppliesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantBusinessSupplies_Create)]
        protected virtual async Task Create(CreateOrEditTenantBusinessSuppliesDto input)
        {
            var tenantBusinessSupplies = ObjectMapper.Map<TenantBusinessSupplies>(input);

            if (AbpSession.TenantId != null)
            {
                tenantBusinessSupplies.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantBusinessSuppliesRepository.InsertAsync(tenantBusinessSupplies);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBusinessSupplies_Edit)]
        protected virtual async Task Update(CreateOrEditTenantBusinessSuppliesDto input)
        {
            var tenantBusinessSupplies = await _tenantBusinessSuppliesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantBusinessSupplies);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBusinessSupplies_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantBusinessSuppliesRepository.DeleteAsync(input.Id);
        }

    }
}