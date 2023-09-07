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
    [AbpAuthorize(AppPermissions.Pages_Title)]
    public class TitleAppService : vitaAppServiceBase, ITitleAppService
    {
        private readonly IRepository<Title> _titleRepository;
        private readonly ITitleExcelExporter _titleExcelExporter;

        public TitleAppService(IRepository<Title> titleRepository, ITitleExcelExporter titleExcelExporter)
        {
            _titleRepository = titleRepository;
            _titleExcelExporter = titleExcelExporter;

        }

        public async Task<PagedResultDto<GetTitleForViewDto>> GetAll(GetAllTitleInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredTitle = _titleRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredTitle = filteredTitle
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var title = from o in pagedAndFilteredTitle
                            select new
                            {

                                o.Name,
                                o.Description,
                                o.IsActive,
                                Id = o.Id
                            };

                var totalCount = await filteredTitle.CountAsync();

                var dbList = await title.ToListAsync();
                var results = new List<GetTitleForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetTitleForViewDto()
                    {
                        Title = new TitleDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetTitleForViewDto>(
                    totalCount,
                    results
                );

            }
        }

        public async Task<GetTitleForViewDto> GetTitleForView(int id)
        {
            var title = await _titleRepository.GetAsync(id);

            var output = new GetTitleForViewDto { Title = ObjectMapper.Map<TitleDto>(title) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Title_Edit)]
        public async Task<GetTitleForEditOutput> GetTitleForEdit(EntityDto input)
        {
            var title = await _titleRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTitleForEditOutput { Title = ObjectMapper.Map<CreateOrEditTitleDto>(title) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTitleDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Title_Create)]
        protected virtual async Task Create(CreateOrEditTitleDto input)
        {
            var title = ObjectMapper.Map<Title>(input);
            title.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                title.TenantId = (int?)AbpSession.TenantId;
            }

            await _titleRepository.InsertAsync(title);

        }

        [AbpAuthorize(AppPermissions.Pages_Title_Edit)]
        protected virtual async Task Update(CreateOrEditTitleDto input)
        {
            var title = await _titleRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, title);

        }

        [AbpAuthorize(AppPermissions.Pages_Title_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _titleRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTitleToExcel(GetAllTitleForExcelInput input)
        {

            var filteredTitle = _titleRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredTitle
                         select new GetTitleForViewDto()
                         {
                             Title = new TitleDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var titleListDtos = await query.ToListAsync();

            return _titleExcelExporter.ExportToFile(titleListDtos);
        }

    }
}