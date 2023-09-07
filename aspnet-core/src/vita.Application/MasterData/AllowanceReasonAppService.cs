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
    [AbpAuthorize(AppPermissions.Pages_AllowanceReason)]
    public class AllowanceReasonAppService : vitaAppServiceBase, IAllowanceReasonAppService
    {
        private readonly IRepository<AllowanceReason> _allowanceReasonRepository;
        private readonly IAllowanceReasonExcelExporter _allowanceReasonExcelExporter;

        public AllowanceReasonAppService(IRepository<AllowanceReason> allowanceReasonRepository, IAllowanceReasonExcelExporter allowanceReasonExcelExporter)
        {
            _allowanceReasonRepository = allowanceReasonRepository;
            _allowanceReasonExcelExporter = allowanceReasonExcelExporter;

        }

        public async Task<PagedResultDto<GetAllowanceReasonForViewDto>> GetAll(GetAllAllowanceReasonInput input)
        {

            var filteredAllowanceReason = _allowanceReasonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredAllowanceReason = filteredAllowanceReason
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var allowanceReason = from o in pagedAndFilteredAllowanceReason
                                  select new
                                  {

                                      o.Name,
                                      o.Description,
                                      o.Code,
                                      o.IsActive,
                                      Id = o.Id
                                  };

            var totalCount = await filteredAllowanceReason.CountAsync();

            var dbList = await allowanceReason.ToListAsync();
            var results = new List<GetAllowanceReasonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAllowanceReasonForViewDto()
                {
                    AllowanceReason = new AllowanceReasonDto
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

            return new PagedResultDto<GetAllowanceReasonForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAllowanceReasonForViewDto> GetAllowanceReasonForView(int id)
        {
            var allowanceReason = await _allowanceReasonRepository.GetAsync(id);

            var output = new GetAllowanceReasonForViewDto { AllowanceReason = ObjectMapper.Map<AllowanceReasonDto>(allowanceReason) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AllowanceReason_Edit)]
        public async Task<GetAllowanceReasonForEditOutput> GetAllowanceReasonForEdit(EntityDto input)
        {
            var allowanceReason = await _allowanceReasonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAllowanceReasonForEditOutput { AllowanceReason = ObjectMapper.Map<CreateOrEditAllowanceReasonDto>(allowanceReason) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAllowanceReasonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AllowanceReason_Create)]
        protected virtual async Task Create(CreateOrEditAllowanceReasonDto input)
        {
            var allowanceReason = ObjectMapper.Map<AllowanceReason>(input);
            allowanceReason.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                allowanceReason.TenantId = (int?)AbpSession.TenantId;
            }

            await _allowanceReasonRepository.InsertAsync(allowanceReason);

        }

        [AbpAuthorize(AppPermissions.Pages_AllowanceReason_Edit)]
        protected virtual async Task Update(CreateOrEditAllowanceReasonDto input)
        {
            var allowanceReason = await _allowanceReasonRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, allowanceReason);

        }

        [AbpAuthorize(AppPermissions.Pages_AllowanceReason_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _allowanceReasonRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAllowanceReasonToExcel(GetAllAllowanceReasonForExcelInput input)
        {

            var filteredAllowanceReason = _allowanceReasonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredAllowanceReason
                         select new GetAllowanceReasonForViewDto()
                         {
                             AllowanceReason = new AllowanceReasonDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var allowanceReasonListDtos = await query.ToListAsync();

            return _allowanceReasonExcelExporter.ExportToFile(allowanceReasonListDtos);
        }

    }
}