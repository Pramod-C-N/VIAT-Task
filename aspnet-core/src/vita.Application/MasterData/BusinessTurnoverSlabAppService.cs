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
    [AbpAuthorize(AppPermissions.Pages_BusinessTurnoverSlab)]
    public class BusinessTurnoverSlabAppService : vitaAppServiceBase, IBusinessTurnoverSlabAppService
    {
        private readonly IRepository<BusinessTurnoverSlab> _businessTurnoverSlabRepository;
        private readonly IBusinessTurnoverSlabExcelExporter _businessTurnoverSlabExcelExporter;

        public BusinessTurnoverSlabAppService(IRepository<BusinessTurnoverSlab> businessTurnoverSlabRepository, IBusinessTurnoverSlabExcelExporter businessTurnoverSlabExcelExporter)
        {
            _businessTurnoverSlabRepository = businessTurnoverSlabRepository;
            _businessTurnoverSlabExcelExporter = businessTurnoverSlabExcelExporter;

        }

        public async Task<PagedResultDto<GetBusinessTurnoverSlabForViewDto>> GetAll(GetAllBusinessTurnoverSlabInput input)
        {

            var filteredBusinessTurnoverSlab = _businessTurnoverSlabRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredBusinessTurnoverSlab = filteredBusinessTurnoverSlab
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessTurnoverSlab = from o in pagedAndFilteredBusinessTurnoverSlab
                                       select new
                                       {

                                           o.Name,
                                           o.Description,
                                           o.Code,
                                           o.IsActive,
                                           Id = o.Id
                                       };

            var totalCount = await filteredBusinessTurnoverSlab.CountAsync();

            var dbList = await businessTurnoverSlab.ToListAsync();
            var results = new List<GetBusinessTurnoverSlabForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessTurnoverSlabForViewDto()
                {
                    BusinessTurnoverSlab = new BusinessTurnoverSlabDto
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

            return new PagedResultDto<GetBusinessTurnoverSlabForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessTurnoverSlabForViewDto> GetBusinessTurnoverSlabForView(int id)
        {
            var businessTurnoverSlab = await _businessTurnoverSlabRepository.GetAsync(id);

            var output = new GetBusinessTurnoverSlabForViewDto { BusinessTurnoverSlab = ObjectMapper.Map<BusinessTurnoverSlabDto>(businessTurnoverSlab) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessTurnoverSlab_Edit)]
        public async Task<GetBusinessTurnoverSlabForEditOutput> GetBusinessTurnoverSlabForEdit(EntityDto input)
        {
            var businessTurnoverSlab = await _businessTurnoverSlabRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessTurnoverSlabForEditOutput { BusinessTurnoverSlab = ObjectMapper.Map<CreateOrEditBusinessTurnoverSlabDto>(businessTurnoverSlab) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessTurnoverSlabDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessTurnoverSlab_Create)]
        protected virtual async Task Create(CreateOrEditBusinessTurnoverSlabDto input)
        {
            var businessTurnoverSlab = ObjectMapper.Map<BusinessTurnoverSlab>(input);
            businessTurnoverSlab.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                businessTurnoverSlab.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessTurnoverSlabRepository.InsertAsync(businessTurnoverSlab);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessTurnoverSlab_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessTurnoverSlabDto input)
        {
            var businessTurnoverSlab = await _businessTurnoverSlabRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, businessTurnoverSlab);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessTurnoverSlab_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _businessTurnoverSlabRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessTurnoverSlabToExcel(GetAllBusinessTurnoverSlabForExcelInput input)
        {

            var filteredBusinessTurnoverSlab = _businessTurnoverSlabRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredBusinessTurnoverSlab
                         select new GetBusinessTurnoverSlabForViewDto()
                         {
                             BusinessTurnoverSlab = new BusinessTurnoverSlabDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var businessTurnoverSlabListDtos = await query.ToListAsync();

            return _businessTurnoverSlabExcelExporter.ExportToFile(businessTurnoverSlabListDtos);
        }

    }
}