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
    [AbpAuthorize(AppPermissions.Pages_ErrorType)]
    public class ErrorTypeAppService : vitaAppServiceBase, IErrorTypeAppService
    {
        private readonly IRepository<ErrorType> _errorTypeRepository;
        private readonly IErrorTypeExcelExporter _errorTypeExcelExporter;

        public ErrorTypeAppService(IRepository<ErrorType> errorTypeRepository, IErrorTypeExcelExporter errorTypeExcelExporter)
        {
            _errorTypeRepository = errorTypeRepository;
            _errorTypeExcelExporter = errorTypeExcelExporter;

        }

        public async Task<PagedResultDto<GetErrorTypeForViewDto>> GetAll(GetAllErrorTypeInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {

                var filteredErrorType = _errorTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.ModuleName.Contains(input.Filter) || e.ErrorGroupId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNameFilter), e => e.ModuleName.Contains(input.ModuleNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorGroupIdFilter), e => e.ErrorGroupId.Contains(input.ErrorGroupIdFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredErrorType = filteredErrorType
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var errorType = from o in pagedAndFilteredErrorType
                                select new
                                {

                                    o.Name,
                                    o.Description,
                                    o.Code,
                                    o.ModuleName,
                                    o.ErrorGroupId,
                                    o.IsActive,
                                    Id = o.Id
                                };

                var totalCount = await filteredErrorType.CountAsync();

                var dbList = await errorType.ToListAsync();
                var results = new List<GetErrorTypeForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetErrorTypeForViewDto()
                    {
                        ErrorType = new ErrorTypeDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            Code = o.Code,
                            ModuleName = o.ModuleName,
                            ErrorGroupId = o.ErrorGroupId,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetErrorTypeForViewDto>(
                    totalCount,
                    results
                );

            }
        }
        public async Task<GetErrorTypeForViewDto> GetErrorTypeForView(int id)
        {
            var errorType = await _errorTypeRepository.GetAsync(id);

            var output = new GetErrorTypeForViewDto { ErrorType = ObjectMapper.Map<ErrorTypeDto>(errorType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ErrorType_Edit)]
        public async Task<GetErrorTypeForEditOutput> GetErrorTypeForEdit(EntityDto input)
        {
            var errorType = await _errorTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetErrorTypeForEditOutput { ErrorType = ObjectMapper.Map<CreateOrEditErrorTypeDto>(errorType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditErrorTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ErrorType_Create)]
        protected virtual async Task Create(CreateOrEditErrorTypeDto input)
        {
            var errorType = ObjectMapper.Map<ErrorType>(input);
            errorType.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                errorType.TenantId = (int?)AbpSession.TenantId;
            }

            await _errorTypeRepository.InsertAsync(errorType);

        }

        [AbpAuthorize(AppPermissions.Pages_ErrorType_Edit)]
        protected virtual async Task Update(CreateOrEditErrorTypeDto input)
        {
            var errorType = await _errorTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, errorType);

        }

        [AbpAuthorize(AppPermissions.Pages_ErrorType_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _errorTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetErrorTypeToExcel(GetAllErrorTypeForExcelInput input)
        {

            var filteredErrorType = _errorTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.ModuleName.Contains(input.Filter) || e.ErrorGroupId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNameFilter), e => e.ModuleName.Contains(input.ModuleNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorGroupIdFilter), e => e.ErrorGroupId.Contains(input.ErrorGroupIdFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredErrorType
                         select new GetErrorTypeForViewDto()
                         {
                             ErrorType = new ErrorTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 ModuleName = o.ModuleName,
                                 ErrorGroupId = o.ErrorGroupId,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var errorTypeListDtos = await query.ToListAsync();

            return _errorTypeExcelExporter.ExportToFile(errorTypeListDtos);
        }

    }
}