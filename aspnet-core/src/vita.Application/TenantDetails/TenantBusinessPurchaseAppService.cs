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
    [AbpAuthorize(AppPermissions.Pages_TenantBusinessPurchase)]
    public class TenantBusinessPurchaseAppService : vitaAppServiceBase, ITenantBusinessPurchaseAppService
    {
        private readonly IRepository<TenantBusinessPurchase> _tenantBusinessPurchaseRepository;

        public TenantBusinessPurchaseAppService(IRepository<TenantBusinessPurchase> tenantBusinessPurchaseRepository)
        {
            _tenantBusinessPurchaseRepository = tenantBusinessPurchaseRepository;

        }

        public async Task<PagedResultDto<GetTenantBusinessPurchaseForViewDto>> GetAll(GetAllTenantBusinessPurchaseInput input)
        {

            var filteredTenantBusinessPurchase = _tenantBusinessPurchaseRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BusinessTypeID.Contains(input.Filter) || e.BusinessPurchase.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessTypeIDFilter), e => e.BusinessTypeID.Contains(input.BusinessTypeIDFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessPurchaseFilter), e => e.BusinessPurchase.Contains(input.BusinessPurchaseFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredTenantBusinessPurchase = filteredTenantBusinessPurchase
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantBusinessPurchase = from o in pagedAndFilteredTenantBusinessPurchase
                                         select new
                                         {

                                             o.BusinessTypeID,
                                             o.BusinessPurchase,
                                             o.IsActive,
                                             Id = o.Id
                                         };

            var totalCount = await filteredTenantBusinessPurchase.CountAsync();

            var dbList = await tenantBusinessPurchase.ToListAsync();
            var results = new List<GetTenantBusinessPurchaseForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantBusinessPurchaseForViewDto()
                {
                    TenantBusinessPurchase = new TenantBusinessPurchaseDto
                    {

                        BusinessTypeID = o.BusinessTypeID,
                        BusinessPurchase = o.BusinessPurchase,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantBusinessPurchaseForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBusinessPurchase_Edit)]
        public async Task<GetTenantBusinessPurchaseForEditOutput> GetTenantBusinessPurchaseForEdit(EntityDto input)
        {
            var tenantBusinessPurchase = await _tenantBusinessPurchaseRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantBusinessPurchaseForEditOutput { TenantBusinessPurchase = ObjectMapper.Map<CreateOrEditTenantBusinessPurchaseDto>(tenantBusinessPurchase) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantBusinessPurchaseDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantBusinessPurchase_Create)]
        protected virtual async Task Create(CreateOrEditTenantBusinessPurchaseDto input)
        {
            var tenantBusinessPurchase = ObjectMapper.Map<TenantBusinessPurchase>(input);

            if (AbpSession.TenantId != null)
            {
                tenantBusinessPurchase.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantBusinessPurchaseRepository.InsertAsync(tenantBusinessPurchase);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBusinessPurchase_Edit)]
        protected virtual async Task Update(CreateOrEditTenantBusinessPurchaseDto input)
        {
            var tenantBusinessPurchase = await _tenantBusinessPurchaseRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantBusinessPurchase);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantBusinessPurchase_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantBusinessPurchaseRepository.DeleteAsync(input.Id);
        }

    }
}