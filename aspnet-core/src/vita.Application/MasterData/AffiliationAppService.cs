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
    [AbpAuthorize(AppPermissions.Pages_Affiliation)]
    public class AffiliationAppService : vitaAppServiceBase, IAffiliationAppService
    {
        private readonly IRepository<Affiliation> _affiliationRepository;
        private readonly IAffiliationExcelExporter _affiliationExcelExporter;

        public AffiliationAppService(IRepository<Affiliation> affiliationRepository, IAffiliationExcelExporter affiliationExcelExporter)
        {
            _affiliationRepository = affiliationRepository;
            _affiliationExcelExporter = affiliationExcelExporter;

        }

        public async Task<PagedResultDto<GetAffiliationForViewDto>> GetAll(GetAllAffiliationInput input)
        {

            var filteredAffiliation = _affiliationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredAffiliation = filteredAffiliation
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var affiliation = from o in pagedAndFilteredAffiliation
                              select new
                              {

                                  o.Name,
                                  o.Description,
                                  o.Code,
                                  o.IsActive,
                                  Id = o.Id
                              };

            var totalCount = await filteredAffiliation.CountAsync();

            var dbList = await affiliation.ToListAsync();
            var results = new List<GetAffiliationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAffiliationForViewDto()
                {
                    Affiliation = new AffiliationDto
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

            return new PagedResultDto<GetAffiliationForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAffiliationForViewDto> GetAffiliationForView(int id)
        {
            var affiliation = await _affiliationRepository.GetAsync(id);

            var output = new GetAffiliationForViewDto { Affiliation = ObjectMapper.Map<AffiliationDto>(affiliation) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Affiliation_Edit)]
        public async Task<GetAffiliationForEditOutput> GetAffiliationForEdit(EntityDto input)
        {
            var affiliation = await _affiliationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAffiliationForEditOutput { Affiliation = ObjectMapper.Map<CreateOrEditAffiliationDto>(affiliation) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAffiliationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Affiliation_Create)]
        protected virtual async Task Create(CreateOrEditAffiliationDto input)
        {
            var affiliation = ObjectMapper.Map<Affiliation>(input);
            affiliation.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                affiliation.TenantId = (int?)AbpSession.TenantId;
            }

            await _affiliationRepository.InsertAsync(affiliation);

        }

        [AbpAuthorize(AppPermissions.Pages_Affiliation_Edit)]
        protected virtual async Task Update(CreateOrEditAffiliationDto input)
        {
            var affiliation = await _affiliationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, affiliation);

        }

        [AbpAuthorize(AppPermissions.Pages_Affiliation_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _affiliationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAffiliationToExcel(GetAllAffiliationForExcelInput input)
        {

            var filteredAffiliation = _affiliationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredAffiliation
                         select new GetAffiliationForViewDto()
                         {
                             Affiliation = new AffiliationDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var affiliationListDtos = await query.ToListAsync();

            return _affiliationExcelExporter.ExportToFile(affiliationListDtos);
        }

    }
}