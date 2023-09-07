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
    [AbpAuthorize(AppPermissions.Pages_ErrorGroup)]
    public class ErrorGroupAppService : vitaAppServiceBase, IErrorGroupAppService
    {
        private readonly IRepository<ErrorGroup> _errorGroupRepository;
        private readonly IErrorGroupExcelExporter _errorGroupExcelExporter;

        public ErrorGroupAppService(IRepository<ErrorGroup> errorGroupRepository, IErrorGroupExcelExporter errorGroupExcelExporter)
        {
            _errorGroupRepository = errorGroupRepository;
            _errorGroupExcelExporter = errorGroupExcelExporter;

        }

        public async Task<PagedResultDto<GetErrorGroupForViewDto>> GetAll(GetAllErrorGroupInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredErrorGroup = _errorGroupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredErrorGroup = filteredErrorGroup
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var errorGroup = from o in pagedAndFilteredErrorGroup
                                 select new
                                 {

                                     o.Name,
                                     o.Description,
                                     o.Code,
                                     o.IsActive,
                                     Id = o.Id
                                 };

                var totalCount = await filteredErrorGroup.CountAsync();

                var dbList = await errorGroup.ToListAsync();
                var results = new List<GetErrorGroupForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetErrorGroupForViewDto()
                    {
                        ErrorGroup = new ErrorGroupDto
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

                return new PagedResultDto<GetErrorGroupForViewDto>(
                    totalCount,
                    results
                );

            }
        }

        public async Task<GetErrorGroupForViewDto> GetErrorGroupForView(int id)
        {
            var errorGroup = await _errorGroupRepository.GetAsync(id);

            var output = new GetErrorGroupForViewDto { ErrorGroup = ObjectMapper.Map<ErrorGroupDto>(errorGroup) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ErrorGroup_Edit)]
        public async Task<GetErrorGroupForEditOutput> GetErrorGroupForEdit(EntityDto input)
        {
            var errorGroup = await _errorGroupRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetErrorGroupForEditOutput { ErrorGroup = ObjectMapper.Map<CreateOrEditErrorGroupDto>(errorGroup) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditErrorGroupDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ErrorGroup_Create)]
        protected virtual async Task Create(CreateOrEditErrorGroupDto input)
        {
            var errorGroup = ObjectMapper.Map<ErrorGroup>(input);
            errorGroup.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                errorGroup.TenantId = (int?)AbpSession.TenantId;
            }

            await _errorGroupRepository.InsertAsync(errorGroup);

        }

        [AbpAuthorize(AppPermissions.Pages_ErrorGroup_Edit)]
        protected virtual async Task Update(CreateOrEditErrorGroupDto input)
        {
            var errorGroup = await _errorGroupRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, errorGroup);

        }

        [AbpAuthorize(AppPermissions.Pages_ErrorGroup_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _errorGroupRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetErrorGroupToExcel(GetAllErrorGroupForExcelInput input)
        {

            var filteredErrorGroup = _errorGroupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredErrorGroup
                         select new GetErrorGroupForViewDto()
                         {
                             ErrorGroup = new ErrorGroupDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var errorGroupListDtos = await query.ToListAsync();

            return _errorGroupExcelExporter.ExportToFile(errorGroupListDtos);
        }

    }
}