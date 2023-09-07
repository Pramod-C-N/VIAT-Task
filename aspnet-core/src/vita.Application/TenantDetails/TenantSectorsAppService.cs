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
    [AbpAuthorize(AppPermissions.Pages_TenantSectors)]
    public class TenantSectorsAppService : vitaAppServiceBase, ITenantSectorsAppService
    {
        private readonly IRepository<TenantSectors> _tenantSectorsRepository;

        public TenantSectorsAppService(IRepository<TenantSectors> tenantSectorsRepository)
        {
            _tenantSectorsRepository = tenantSectorsRepository;

        }

        public async Task<PagedResultDto<GetTenantSectorsForViewDto>> GetAll(GetAllTenantSectorsInput input)
        {

            var filteredTenantSectors = _tenantSectorsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SubIndustryCode.Contains(input.Filter) || e.SubIndustryName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryCodeFilter), e => e.SubIndustryCode.Contains(input.SubIndustryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryNameFilter), e => e.SubIndustryName.Contains(input.SubIndustryNameFilter));

            var pagedAndFilteredTenantSectors = filteredTenantSectors
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantSectors = from o in pagedAndFilteredTenantSectors
                                select new
                                {

                                    o.SubIndustryCode,
                                    o.SubIndustryName,
                                    Id = o.Id
                                };

            var totalCount = await filteredTenantSectors.CountAsync();

            var dbList = await tenantSectors.ToListAsync();
            var results = new List<GetTenantSectorsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantSectorsForViewDto()
                {
                    TenantSectors = new TenantSectorsDto
                    {

                        SubIndustryCode = o.SubIndustryCode,
                        SubIndustryName = o.SubIndustryName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantSectorsForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TenantSectors_Edit)]
        public async Task<GetTenantSectorsForEditOutput> GetTenantSectorsForEdit(EntityDto input)
        {
            var tenantSectors = await _tenantSectorsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantSectorsForEditOutput { TenantSectors = ObjectMapper.Map<CreateOrEditTenantSectorsDto>(tenantSectors) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantSectorsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantSectors_Create)]
        protected virtual async Task Create(CreateOrEditTenantSectorsDto input)
        {
            var tenantSectors = ObjectMapper.Map<TenantSectors>(input);

            if (AbpSession.TenantId != null)
            {
                tenantSectors.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantSectorsRepository.InsertAsync(tenantSectors);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantSectors_Edit)]
        protected virtual async Task Update(CreateOrEditTenantSectorsDto input)
        {
            var tenantSectors = await _tenantSectorsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantSectors);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantSectors_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantSectorsRepository.DeleteAsync(input.Id);
        }

    }
}