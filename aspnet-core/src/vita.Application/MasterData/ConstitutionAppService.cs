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
    [AbpAuthorize(AppPermissions.Pages_Constitution)]
    public class ConstitutionAppService : vitaAppServiceBase, IConstitutionAppService
    {
        private readonly IRepository<Constitution> _constitutionRepository;
        private readonly IConstitutionExcelExporter _constitutionExcelExporter;

        public ConstitutionAppService(IRepository<Constitution> constitutionRepository, IConstitutionExcelExporter constitutionExcelExporter)
        {
            _constitutionRepository = constitutionRepository;
            _constitutionExcelExporter = constitutionExcelExporter;

        }

        public async Task<PagedResultDto<GetConstitutionForViewDto>> GetAll(GetAllConstitutionInput input)
        {

            var filteredConstitution = _constitutionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredConstitution = filteredConstitution
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var constitution = from o in pagedAndFilteredConstitution
                               select new
                               {

                                   o.Name,
                                   o.Description,
                                   o.Code,
                                   o.IsActive,
                                   Id = o.Id
                               };

            var totalCount = await filteredConstitution.CountAsync();

            var dbList = await constitution.ToListAsync();
            var results = new List<GetConstitutionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetConstitutionForViewDto()
                {
                    Constitution = new ConstitutionDto
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

            return new PagedResultDto<GetConstitutionForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetConstitutionForViewDto> GetConstitutionForView(int id)
        {
            var constitution = await _constitutionRepository.GetAsync(id);

            var output = new GetConstitutionForViewDto { Constitution = ObjectMapper.Map<ConstitutionDto>(constitution) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Constitution_Edit)]
        public async Task<GetConstitutionForEditOutput> GetConstitutionForEdit(EntityDto input)
        {
            var constitution = await _constitutionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetConstitutionForEditOutput { Constitution = ObjectMapper.Map<CreateOrEditConstitutionDto>(constitution) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditConstitutionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Constitution_Create)]
        protected virtual async Task Create(CreateOrEditConstitutionDto input)
        {
            var constitution = ObjectMapper.Map<Constitution>(input);
            constitution.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                constitution.TenantId = (int?)AbpSession.TenantId;
            }

            await _constitutionRepository.InsertAsync(constitution);

        }

        [AbpAuthorize(AppPermissions.Pages_Constitution_Edit)]
        protected virtual async Task Update(CreateOrEditConstitutionDto input)
        {
            var constitution = await _constitutionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, constitution);

        }

        [AbpAuthorize(AppPermissions.Pages_Constitution_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _constitutionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetConstitutionToExcel(GetAllConstitutionForExcelInput input)
        {

            var filteredConstitution = _constitutionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredConstitution
                         select new GetConstitutionForViewDto()
                         {
                             Constitution = new ConstitutionDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var constitutionListDtos = await query.ToListAsync();

            return _constitutionExcelExporter.ExportToFile(constitutionListDtos);
        }

    }
}