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
    [AbpAuthorize(AppPermissions.Pages_Sector)]
    public class SectorAppService : vitaAppServiceBase, ISectorAppService
    {
        private readonly IRepository<Sector> _sectorRepository;
        private readonly ISectorExcelExporter _sectorExcelExporter;

        public SectorAppService(IRepository<Sector> sectorRepository, ISectorExcelExporter sectorExcelExporter)
        {
            _sectorRepository = sectorRepository;
            _sectorExcelExporter = sectorExcelExporter;

        }

        public async Task<PagedResultDto<GetSectorForViewDto>> GetAll(GetAllSectorInput input)
        {

            var filteredSector = _sectorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.GroupName.Contains(input.Filter) || e.IndustryGroupCode.Contains(input.Filter) || e.IndustryGroupName.Contains(input.Filter) || e.IndustryCode.Contains(input.Filter) || e.IndustryName.Contains(input.Filter) || e.SubIndustryCode.Contains(input.Filter) || e.SubIndustryName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupNameFilter), e => e.GroupName.Contains(input.GroupNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryGroupCodeFilter), e => e.IndustryGroupCode.Contains(input.IndustryGroupCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryGroupNameFilter), e => e.IndustryGroupName.Contains(input.IndustryGroupNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryCodeFilter), e => e.IndustryCode.Contains(input.IndustryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryNameFilter), e => e.IndustryName.Contains(input.IndustryNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryCodeFilter), e => e.SubIndustryCode.Contains(input.SubIndustryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryNameFilter), e => e.SubIndustryName.Contains(input.SubIndustryNameFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredSector = filteredSector
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var sector = from o in pagedAndFilteredSector
                         select new
                         {

                             o.Name,
                             o.Description,
                             o.Code,
                             o.GroupName,
                             o.IndustryGroupCode,
                             o.IndustryGroupName,
                             o.IndustryCode,
                             o.IndustryName,
                             o.SubIndustryCode,
                             o.SubIndustryName,
                             o.IsActive,
                             Id = o.Id
                         };

            var totalCount = await filteredSector.CountAsync();

            var dbList = await sector.ToListAsync();
            var results = new List<GetSectorForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSectorForViewDto()
                {
                    Sector = new SectorDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Code = o.Code,
                        GroupName = o.GroupName,
                        IndustryGroupCode = o.IndustryGroupCode,
                        IndustryGroupName = o.IndustryGroupName,
                        IndustryCode = o.IndustryCode,
                        IndustryName = o.IndustryName,
                        SubIndustryCode = o.SubIndustryCode,
                        SubIndustryName = o.SubIndustryName,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSectorForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSectorForViewDto> GetSectorForView(int id)
        {
            var sector = await _sectorRepository.GetAsync(id);

            var output = new GetSectorForViewDto { Sector = ObjectMapper.Map<SectorDto>(sector) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Sector_Edit)]
        public async Task<GetSectorForEditOutput> GetSectorForEdit(EntityDto input)
        {
            var sector = await _sectorRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSectorForEditOutput { Sector = ObjectMapper.Map<CreateOrEditSectorDto>(sector) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSectorDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Sector_Create)]
        protected virtual async Task Create(CreateOrEditSectorDto input)
        {
            var sector = ObjectMapper.Map<Sector>(input);
            sector.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                sector.TenantId = (int?)AbpSession.TenantId;
            }

            await _sectorRepository.InsertAsync(sector);

        }

        [AbpAuthorize(AppPermissions.Pages_Sector_Edit)]
        protected virtual async Task Update(CreateOrEditSectorDto input)
        {
            var sector = await _sectorRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, sector);

        }

        [AbpAuthorize(AppPermissions.Pages_Sector_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _sectorRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSectorToExcel(GetAllSectorForExcelInput input)
        {

            var filteredSector = _sectorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.GroupName.Contains(input.Filter) || e.IndustryGroupCode.Contains(input.Filter) || e.IndustryGroupName.Contains(input.Filter) || e.IndustryCode.Contains(input.Filter) || e.IndustryName.Contains(input.Filter) || e.SubIndustryCode.Contains(input.Filter) || e.SubIndustryName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupNameFilter), e => e.GroupName.Contains(input.GroupNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryGroupCodeFilter), e => e.IndustryGroupCode.Contains(input.IndustryGroupCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryGroupNameFilter), e => e.IndustryGroupName.Contains(input.IndustryGroupNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryCodeFilter), e => e.IndustryCode.Contains(input.IndustryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryNameFilter), e => e.IndustryName.Contains(input.IndustryNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryCodeFilter), e => e.SubIndustryCode.Contains(input.SubIndustryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubIndustryNameFilter), e => e.SubIndustryName.Contains(input.SubIndustryNameFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredSector
                         select new GetSectorForViewDto()
                         {
                             Sector = new SectorDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 GroupName = o.GroupName,
                                 IndustryGroupCode = o.IndustryGroupCode,
                                 IndustryGroupName = o.IndustryGroupName,
                                 IndustryCode = o.IndustryCode,
                                 IndustryName = o.IndustryName,
                                 SubIndustryCode = o.SubIndustryCode,
                                 SubIndustryName = o.SubIndustryName,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var sectorListDtos = await query.ToListAsync();

            return _sectorExcelExporter.ExportToFile(sectorListDtos);
        }

    }
}