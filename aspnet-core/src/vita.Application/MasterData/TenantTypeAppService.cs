using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.MasterData.Exporting;
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
    [AbpAuthorize(AppPermissions.Pages_TenantType)]
    public class TenantTypeAppService : vitaAppServiceBase, ITenantTypeAppService
    {
        private readonly IRepository<TenantType> _tenantTypeRepository;
        private readonly ITenantTypeExcelExporter _tenantTypeExcelExporter;

        public TenantTypeAppService(IRepository<TenantType> tenantTypeRepository, ITenantTypeExcelExporter tenantTypeExcelExporter)
        {
            _tenantTypeRepository = tenantTypeRepository;
            _tenantTypeExcelExporter = tenantTypeExcelExporter;

        }

        public async Task<PagedResultDto<GetTenantTypeForViewDto>> GetAll(GetAllTenantTypeInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredTenantType = _tenantTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredTenantType = filteredTenantType
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var tenantType = from o in pagedAndFilteredTenantType
                                 select new
                                 {

                                     o.Name,
                                     o.Description,
                                     o.IsActive,
                                     Id = o.Id
                                 };

                var totalCount = await filteredTenantType.CountAsync();

                var dbList = await tenantType.ToListAsync();
                var results = new List<GetTenantTypeForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetTenantTypeForViewDto()
                    {
                        TenantType = new TenantTypeDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetTenantTypeForViewDto>(
                    totalCount,
                    results
                );
            }

        }

        public async Task<GetTenantTypeForViewDto> GetTenantTypeForView(int id)
        {
            var tenantType = await _tenantTypeRepository.GetAsync(id);

            var output = new GetTenantTypeForViewDto { TenantType = ObjectMapper.Map<TenantTypeDto>(tenantType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantType_Edit)]
        public async Task<GetTenantTypeForEditOutput> GetTenantTypeForEdit(EntityDto input)
        {
            var tenantType = await _tenantTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantTypeForEditOutput { TenantType = ObjectMapper.Map<CreateOrEditTenantTypeDto>(tenantType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantType_Create)]
        protected virtual async Task Create(CreateOrEditTenantTypeDto input)
        {
            var tenantType = ObjectMapper.Map<TenantType>(input);
            tenantType.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                tenantType.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantTypeRepository.InsertAsync(tenantType);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantType_Edit)]
        protected virtual async Task Update(CreateOrEditTenantTypeDto input)
        {
            var tenantType = await _tenantTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tenantType);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantType_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTenantTypeToExcel(GetAllTenantTypeForExcelInput input)
        {

            var filteredTenantType = _tenantTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredTenantType
                         select new GetTenantTypeForViewDto()
                         {
                             TenantType = new TenantTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var tenantTypeListDtos = await query.ToListAsync();

            return _tenantTypeExcelExporter.ExportToFile(tenantTypeListDtos);
        }

    }
}