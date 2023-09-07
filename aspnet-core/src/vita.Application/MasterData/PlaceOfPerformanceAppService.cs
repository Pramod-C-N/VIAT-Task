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
    [AbpAuthorize(AppPermissions.Pages_PlaceOfPerformance)]
    public class PlaceOfPerformanceAppService : vitaAppServiceBase, IPlaceOfPerformanceAppService
    {
        private readonly IRepository<PlaceOfPerformance> _placeOfPerformanceRepository;
        private readonly IPlaceOfPerformanceExcelExporter _placeOfPerformanceExcelExporter;

        public PlaceOfPerformanceAppService(IRepository<PlaceOfPerformance> placeOfPerformanceRepository, IPlaceOfPerformanceExcelExporter placeOfPerformanceExcelExporter)
        {
            _placeOfPerformanceRepository = placeOfPerformanceRepository;
            _placeOfPerformanceExcelExporter = placeOfPerformanceExcelExporter;

        }

        public async Task<PagedResultDto<GetPlaceOfPerformanceForViewDto>> GetAll(GetAllPlaceOfPerformanceInput input)
        {

            var filteredPlaceOfPerformance = _placeOfPerformanceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredPlaceOfPerformance = filteredPlaceOfPerformance
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var placeOfPerformance = from o in pagedAndFilteredPlaceOfPerformance
                                     select new
                                     {

                                         o.Name,
                                         o.Description,
                                         o.Code,
                                         o.IsActive,
                                         Id = o.Id
                                     };

            var totalCount = await filteredPlaceOfPerformance.CountAsync();

            var dbList = await placeOfPerformance.ToListAsync();
            var results = new List<GetPlaceOfPerformanceForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPlaceOfPerformanceForViewDto()
                {
                    PlaceOfPerformance = new PlaceOfPerformanceDto
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

            return new PagedResultDto<GetPlaceOfPerformanceForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPlaceOfPerformanceForViewDto> GetPlaceOfPerformanceForView(int id)
        {
            var placeOfPerformance = await _placeOfPerformanceRepository.GetAsync(id);

            var output = new GetPlaceOfPerformanceForViewDto { PlaceOfPerformance = ObjectMapper.Map<PlaceOfPerformanceDto>(placeOfPerformance) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PlaceOfPerformance_Edit)]
        public async Task<GetPlaceOfPerformanceForEditOutput> GetPlaceOfPerformanceForEdit(EntityDto input)
        {
            var placeOfPerformance = await _placeOfPerformanceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPlaceOfPerformanceForEditOutput { PlaceOfPerformance = ObjectMapper.Map<CreateOrEditPlaceOfPerformanceDto>(placeOfPerformance) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPlaceOfPerformanceDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PlaceOfPerformance_Create)]
        protected virtual async Task Create(CreateOrEditPlaceOfPerformanceDto input)
        {
            var placeOfPerformance = ObjectMapper.Map<PlaceOfPerformance>(input);
            placeOfPerformance.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                placeOfPerformance.TenantId = (int?)AbpSession.TenantId;
            }

            await _placeOfPerformanceRepository.InsertAsync(placeOfPerformance);

        }

        [AbpAuthorize(AppPermissions.Pages_PlaceOfPerformance_Edit)]
        protected virtual async Task Update(CreateOrEditPlaceOfPerformanceDto input)
        {
            var placeOfPerformance = await _placeOfPerformanceRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, placeOfPerformance);

        }

        [AbpAuthorize(AppPermissions.Pages_PlaceOfPerformance_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _placeOfPerformanceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPlaceOfPerformanceToExcel(GetAllPlaceOfPerformanceForExcelInput input)
        {

            var filteredPlaceOfPerformance = _placeOfPerformanceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredPlaceOfPerformance
                         select new GetPlaceOfPerformanceForViewDto()
                         {
                             PlaceOfPerformance = new PlaceOfPerformanceDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var placeOfPerformanceListDtos = await query.ToListAsync();

            return _placeOfPerformanceExcelExporter.ExportToFile(placeOfPerformanceListDtos);
        }

    }
}