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
    [AbpAuthorize(AppPermissions.Pages_Gender)]
    public class GenderAppService : vitaAppServiceBase, IGenderAppService
    {
        private readonly IRepository<Gender> _genderRepository;
        private readonly IGenderExcelExporter _genderExcelExporter;

        public GenderAppService(IRepository<Gender> genderRepository, IGenderExcelExporter genderExcelExporter)
        {
            _genderRepository = genderRepository;
            _genderExcelExporter = genderExcelExporter;

        }

        public async Task<PagedResultDto<GetGenderForViewDto>> GetAll(GetAllGenderInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredGender = _genderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredGender = filteredGender
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var gender = from o in pagedAndFilteredGender
                             select new
                             {

                                 o.Name,
                                 o.IsActive,
                                 Id = o.Id
                             };

                var totalCount = await filteredGender.CountAsync();

                var dbList = await gender.ToListAsync();
                var results = new List<GetGenderForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetGenderForViewDto()
                    {
                        Gender = new GenderDto
                        {

                            Name = o.Name,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetGenderForViewDto>(
                    totalCount,
                    results
                );
            }
        }

        public async Task<GetGenderForViewDto> GetGenderForView(int id)
        {
            var gender = await _genderRepository.GetAsync(id);

            var output = new GetGenderForViewDto { Gender = ObjectMapper.Map<GenderDto>(gender) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Gender_Edit)]
        public async Task<GetGenderForEditOutput> GetGenderForEdit(EntityDto input)
        {
            var gender = await _genderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGenderForEditOutput { Gender = ObjectMapper.Map<CreateOrEditGenderDto>(gender) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGenderDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Gender_Create)]
        protected virtual async Task Create(CreateOrEditGenderDto input)
        {
            var gender = ObjectMapper.Map<Gender>(input);
            gender.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                gender.TenantId = (int?)AbpSession.TenantId;
            }

            await _genderRepository.InsertAsync(gender);

        }

        [AbpAuthorize(AppPermissions.Pages_Gender_Edit)]
        protected virtual async Task Update(CreateOrEditGenderDto input)
        {
            var gender = await _genderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, gender);

        }

        [AbpAuthorize(AppPermissions.Pages_Gender_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _genderRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetGenderToExcel(GetAllGenderForExcelInput input)
        {

            var filteredGender = _genderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredGender
                         select new GetGenderForViewDto()
                         {
                             Gender = new GenderDto
                             {
                                 Name = o.Name,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var genderListDtos = await query.ToListAsync();

            return _genderExcelExporter.ExportToFile(genderListDtos);
        }

    }
}