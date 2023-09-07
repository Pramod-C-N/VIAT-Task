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
    [AbpAuthorize(AppPermissions.Pages_NatureofServices)]
    public class NatureofServicesAppService : vitaAppServiceBase, INatureofServicesAppService
    {
        private readonly IRepository<NatureofServices> _natureofServicesRepository;
        private readonly INatureofServicesExcelExporter _natureofServicesExcelExporter;

        public NatureofServicesAppService(IRepository<NatureofServices> natureofServicesRepository, INatureofServicesExcelExporter natureofServicesExcelExporter)
        {
            _natureofServicesRepository = natureofServicesRepository;
            _natureofServicesExcelExporter = natureofServicesExcelExporter;

        }

        public async Task<PagedResultDto<GetNatureofServicesForViewDto>> GetAll(GetAllNatureofServicesInput input)
        {

            var filteredNatureofServices = _natureofServicesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredNatureofServices = filteredNatureofServices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var natureofServices = from o in pagedAndFilteredNatureofServices
                                   select new
                                   {

                                       o.Name,
                                       o.Description,
                                       o.Code,
                                       o.IsActive,
                                       Id = o.Id
                                   };

            var totalCount = await filteredNatureofServices.CountAsync();

            var dbList = await natureofServices.ToListAsync();
            var results = new List<GetNatureofServicesForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetNatureofServicesForViewDto()
                {
                    NatureofServices = new NatureofServicesDto
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

            return new PagedResultDto<GetNatureofServicesForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetNatureofServicesForViewDto> GetNatureofServicesForView(int id)
        {
            var natureofServices = await _natureofServicesRepository.GetAsync(id);

            var output = new GetNatureofServicesForViewDto { NatureofServices = ObjectMapper.Map<NatureofServicesDto>(natureofServices) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_NatureofServices_Edit)]
        public async Task<GetNatureofServicesForEditOutput> GetNatureofServicesForEdit(EntityDto input)
        {
            var natureofServices = await _natureofServicesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetNatureofServicesForEditOutput { NatureofServices = ObjectMapper.Map<CreateOrEditNatureofServicesDto>(natureofServices) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditNatureofServicesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_NatureofServices_Create)]
        protected virtual async Task Create(CreateOrEditNatureofServicesDto input)
        {
            var natureofServices = ObjectMapper.Map<NatureofServices>(input);
            natureofServices.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                natureofServices.TenantId = (int?)AbpSession.TenantId;
            }

            await _natureofServicesRepository.InsertAsync(natureofServices);

        }

        [AbpAuthorize(AppPermissions.Pages_NatureofServices_Edit)]
        protected virtual async Task Update(CreateOrEditNatureofServicesDto input)
        {
            var natureofServices = await _natureofServicesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, natureofServices);

        }

        [AbpAuthorize(AppPermissions.Pages_NatureofServices_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _natureofServicesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetNatureofServicesToExcel(GetAllNatureofServicesForExcelInput input)
        {

            var filteredNatureofServices = _natureofServicesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredNatureofServices
                         select new GetNatureofServicesForViewDto()
                         {
                             NatureofServices = new NatureofServicesDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var natureofServicesListDtos = await query.ToListAsync();

            return _natureofServicesExcelExporter.ExportToFile(natureofServicesListDtos);
        }

    }
}