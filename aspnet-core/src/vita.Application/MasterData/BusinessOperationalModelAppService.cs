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
    [AbpAuthorize(AppPermissions.Pages_BusinessOperationalModel)]
    public class BusinessOperationalModelAppService : vitaAppServiceBase, IBusinessOperationalModelAppService
    {
        private readonly IRepository<BusinessOperationalModel> _businessOperationalModelRepository;
        private readonly IBusinessOperationalModelExcelExporter _businessOperationalModelExcelExporter;

        public BusinessOperationalModelAppService(IRepository<BusinessOperationalModel> businessOperationalModelRepository, IBusinessOperationalModelExcelExporter businessOperationalModelExcelExporter)
        {
            _businessOperationalModelRepository = businessOperationalModelRepository;
            _businessOperationalModelExcelExporter = businessOperationalModelExcelExporter;

        }

        public async Task<PagedResultDto<GetBusinessOperationalModelForViewDto>> GetAll(GetAllBusinessOperationalModelInput input)
        {

            var filteredBusinessOperationalModel = _businessOperationalModelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredBusinessOperationalModel = filteredBusinessOperationalModel
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessOperationalModel = from o in pagedAndFilteredBusinessOperationalModel
                                           select new
                                           {

                                               o.Name,
                                               o.Description,
                                               o.Code,
                                               o.IsActive,
                                               Id = o.Id
                                           };

            var totalCount = await filteredBusinessOperationalModel.CountAsync();

            var dbList = await businessOperationalModel.ToListAsync();
            var results = new List<GetBusinessOperationalModelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessOperationalModelForViewDto()
                {
                    BusinessOperationalModel = new BusinessOperationalModelDto
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

            return new PagedResultDto<GetBusinessOperationalModelForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessOperationalModelForViewDto> GetBusinessOperationalModelForView(int id)
        {
            var businessOperationalModel = await _businessOperationalModelRepository.GetAsync(id);

            var output = new GetBusinessOperationalModelForViewDto { BusinessOperationalModel = ObjectMapper.Map<BusinessOperationalModelDto>(businessOperationalModel) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessOperationalModel_Edit)]
        public async Task<GetBusinessOperationalModelForEditOutput> GetBusinessOperationalModelForEdit(EntityDto input)
        {
            var businessOperationalModel = await _businessOperationalModelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessOperationalModelForEditOutput { BusinessOperationalModel = ObjectMapper.Map<CreateOrEditBusinessOperationalModelDto>(businessOperationalModel) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessOperationalModelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessOperationalModel_Create)]
        protected virtual async Task Create(CreateOrEditBusinessOperationalModelDto input)
        {
            var businessOperationalModel = ObjectMapper.Map<BusinessOperationalModel>(input);
            businessOperationalModel.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                businessOperationalModel.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessOperationalModelRepository.InsertAsync(businessOperationalModel);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessOperationalModel_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessOperationalModelDto input)
        {
            var businessOperationalModel = await _businessOperationalModelRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, businessOperationalModel);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessOperationalModel_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _businessOperationalModelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessOperationalModelToExcel(GetAllBusinessOperationalModelForExcelInput input)
        {

            var filteredBusinessOperationalModel = _businessOperationalModelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredBusinessOperationalModel
                         select new GetBusinessOperationalModelForViewDto()
                         {
                             BusinessOperationalModel = new BusinessOperationalModelDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var businessOperationalModelListDtos = await query.ToListAsync();

            return _businessOperationalModelExcelExporter.ExportToFile(businessOperationalModelListDtos);
        }

    }
}