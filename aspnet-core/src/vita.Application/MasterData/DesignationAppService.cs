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
    [AbpAuthorize(AppPermissions.Pages_Designation)]
    public class DesignationAppService : vitaAppServiceBase, IDesignationAppService
    {
        private readonly IRepository<Designation> _designationRepository;
        private readonly IDesignationExcelExporter _designationExcelExporter;

        public DesignationAppService(IRepository<Designation> designationRepository, IDesignationExcelExporter designationExcelExporter)
        {
            _designationRepository = designationRepository;
            _designationExcelExporter = designationExcelExporter;

        }

        public async Task<PagedResultDto<GetDesignationForViewDto>> GetAll(GetAllDesignationInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {

                var filteredDesignation = _designationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredDesignation = filteredDesignation
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var designation = from o in pagedAndFilteredDesignation
                                  select new
                                  {

                                      o.Name,
                                      o.Description,
                                      o.Code,
                                      o.IsActive,
                                      Id = o.Id
                                  };

                var totalCount = await filteredDesignation.CountAsync();

                var dbList = await designation.ToListAsync();
                var results = new List<GetDesignationForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetDesignationForViewDto()
                    {
                        Designation = new DesignationDto
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

                return new PagedResultDto<GetDesignationForViewDto>(
                    totalCount,
                    results
                );

            }
        }

        public async Task<GetDesignationForViewDto> GetDesignationForView(int id)
        {
            var designation = await _designationRepository.GetAsync(id);

            var output = new GetDesignationForViewDto { Designation = ObjectMapper.Map<DesignationDto>(designation) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Designation_Edit)]
        public async Task<GetDesignationForEditOutput> GetDesignationForEdit(EntityDto input)
        {
            var designation = await _designationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDesignationForEditOutput { Designation = ObjectMapper.Map<CreateOrEditDesignationDto>(designation) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDesignationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Designation_Create)]
        protected virtual async Task Create(CreateOrEditDesignationDto input)
        {
            var designation = ObjectMapper.Map<Designation>(input);

            if (AbpSession.TenantId != null)
            {
                designation.TenantId = (int?)AbpSession.TenantId;
            }

            await _designationRepository.InsertAsync(designation);

        }

        [AbpAuthorize(AppPermissions.Pages_Designation_Edit)]
        protected virtual async Task Update(CreateOrEditDesignationDto input)
        {
            var designation = await _designationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, designation);

        }

        [AbpAuthorize(AppPermissions.Pages_Designation_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _designationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDesignationToExcel(GetAllDesignationForExcelInput input)
        {

            var filteredDesignation = _designationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredDesignation
                         select new GetDesignationForViewDto()
                         {
                             Designation = new DesignationDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var designationListDtos = await query.ToListAsync();

            return _designationExcelExporter.ExportToFile(designationListDtos);
        }

    }
}