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
    [AbpAuthorize(AppPermissions.Pages_UnitOfMeasurement)]
    public class UnitOfMeasurementAppService : vitaAppServiceBase, IUnitOfMeasurementAppService
    {
        private readonly IRepository<UnitOfMeasurement> _unitOfMeasurementRepository;
        private readonly IUnitOfMeasurementExcelExporter _unitOfMeasurementExcelExporter;

        public UnitOfMeasurementAppService(IRepository<UnitOfMeasurement> unitOfMeasurementRepository, IUnitOfMeasurementExcelExporter unitOfMeasurementExcelExporter)
        {
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _unitOfMeasurementExcelExporter = unitOfMeasurementExcelExporter;

        }

        public async Task<PagedResultDto<GetUnitOfMeasurementForViewDto>> GetAll(GetAllUnitOfMeasurementInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredUnitOfMeasurement = _unitOfMeasurementRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredUnitOfMeasurement = filteredUnitOfMeasurement
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var unitOfMeasurement = from o in pagedAndFilteredUnitOfMeasurement
                                        select new
                                        {

                                            o.Name,
                                            o.Description,
                                            o.Code,
                                            o.IsActive,
                                            Id = o.Id
                                        };

                var totalCount = await filteredUnitOfMeasurement.CountAsync();

                var dbList = await unitOfMeasurement.ToListAsync();
                var results = new List<GetUnitOfMeasurementForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetUnitOfMeasurementForViewDto()
                    {
                        UnitOfMeasurement = new UnitOfMeasurementDto
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

                return new PagedResultDto<GetUnitOfMeasurementForViewDto>(
                    totalCount,
                    results
                );
            }
        }

        public async Task<GetUnitOfMeasurementForViewDto> GetUnitOfMeasurementForView(int id)
        {
            var unitOfMeasurement = await _unitOfMeasurementRepository.GetAsync(id);

            var output = new GetUnitOfMeasurementForViewDto { UnitOfMeasurement = ObjectMapper.Map<UnitOfMeasurementDto>(unitOfMeasurement) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_UnitOfMeasurement_Edit)]
        public async Task<GetUnitOfMeasurementForEditOutput> GetUnitOfMeasurementForEdit(EntityDto input)
        {
            var unitOfMeasurement = await _unitOfMeasurementRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetUnitOfMeasurementForEditOutput { UnitOfMeasurement = ObjectMapper.Map<CreateOrEditUnitOfMeasurementDto>(unitOfMeasurement) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditUnitOfMeasurementDto input)
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

        [AbpAuthorize(AppPermissions.Pages_UnitOfMeasurement_Create)]
        protected virtual async Task Create(CreateOrEditUnitOfMeasurementDto input)
        {
            var unitOfMeasurement = ObjectMapper.Map<UnitOfMeasurement>(input);
            unitOfMeasurement.UniqueIdentifier=Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                unitOfMeasurement.TenantId = (int?)AbpSession.TenantId;
            }

            await _unitOfMeasurementRepository.InsertAsync(unitOfMeasurement);

        }

        [AbpAuthorize(AppPermissions.Pages_UnitOfMeasurement_Edit)]
        protected virtual async Task Update(CreateOrEditUnitOfMeasurementDto input)
        {
            var unitOfMeasurement = await _unitOfMeasurementRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, unitOfMeasurement);

        }

        [AbpAuthorize(AppPermissions.Pages_UnitOfMeasurement_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _unitOfMeasurementRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetUnitOfMeasurementToExcel(GetAllUnitOfMeasurementForExcelInput input)
        {

            var filteredUnitOfMeasurement = _unitOfMeasurementRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredUnitOfMeasurement
                         select new GetUnitOfMeasurementForViewDto()
                         {
                             UnitOfMeasurement = new UnitOfMeasurementDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var unitOfMeasurementListDtos = await query.ToListAsync();

            return _unitOfMeasurementExcelExporter.ExportToFile(unitOfMeasurementListDtos);
        }

    }
}