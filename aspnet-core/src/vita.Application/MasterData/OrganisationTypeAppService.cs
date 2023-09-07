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
    [AbpAuthorize(AppPermissions.Pages_OrganisationType)]
    public class OrganisationTypeAppService : vitaAppServiceBase, IOrganisationTypeAppService
    {
        private readonly IRepository<OrganisationType> _organisationTypeRepository;
        private readonly IOrganisationTypeExcelExporter _organisationTypeExcelExporter;

        public OrganisationTypeAppService(IRepository<OrganisationType> organisationTypeRepository, IOrganisationTypeExcelExporter organisationTypeExcelExporter)
        {
            _organisationTypeRepository = organisationTypeRepository;
            _organisationTypeExcelExporter = organisationTypeExcelExporter;

        }

        public async Task<PagedResultDto<GetOrganisationTypeForViewDto>> GetAll(GetAllOrganisationTypeInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredOrganisationType = _organisationTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredOrganisationType = filteredOrganisationType
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var organisationType = from o in pagedAndFilteredOrganisationType
                                       select new
                                       {

                                           o.Name,
                                           o.Description,
                                           o.Code,
                                           o.IsActive,
                                           Id = o.Id
                                       };

                var totalCount = await filteredOrganisationType.CountAsync();

                var dbList = await organisationType.ToListAsync();
                var results = new List<GetOrganisationTypeForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetOrganisationTypeForViewDto()
                    {
                        OrganisationType = new OrganisationTypeDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            Code = o.Code,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetOrganisationTypeForViewDto>(
                    totalCount,
                    results
                );
            }

        }

        public async Task<GetOrganisationTypeForViewDto> GetOrganisationTypeForView(int id)
        {
            var organisationType = await _organisationTypeRepository.GetAsync(id);

            var output = new GetOrganisationTypeForViewDto { OrganisationType = ObjectMapper.Map<OrganisationTypeDto>(organisationType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrganisationType_Edit)]
        public async Task<GetOrganisationTypeForEditOutput> GetOrganisationTypeForEdit(EntityDto input)
        {
            var organisationType = await _organisationTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrganisationTypeForEditOutput { OrganisationType = ObjectMapper.Map<CreateOrEditOrganisationTypeDto>(organisationType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrganisationTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrganisationType_Create)]
        protected virtual async Task Create(CreateOrEditOrganisationTypeDto input)
        {
            var organisationType = ObjectMapper.Map<OrganisationType>(input);
            organisationType.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                organisationType.TenantId = (int?)AbpSession.TenantId;
            }

            await _organisationTypeRepository.InsertAsync(organisationType);

        }

        [AbpAuthorize(AppPermissions.Pages_OrganisationType_Edit)]
        protected virtual async Task Update(CreateOrEditOrganisationTypeDto input)
        {
            var organisationType = await _organisationTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, organisationType);

        }

        [AbpAuthorize(AppPermissions.Pages_OrganisationType_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _organisationTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrganisationTypeToExcel(GetAllOrganisationTypeForExcelInput input)
        {

            var filteredOrganisationType = _organisationTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredOrganisationType
                         select new GetOrganisationTypeForViewDto()
                         {
                             OrganisationType = new OrganisationTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var organisationTypeListDtos = await query.ToListAsync();

            return _organisationTypeExcelExporter.ExportToFile(organisationTypeListDtos);
        }

    }
}