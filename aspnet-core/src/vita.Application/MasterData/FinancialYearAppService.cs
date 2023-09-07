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
    [AbpAuthorize(AppPermissions.Pages_FinancialYear)]
    public class FinancialYearAppService : vitaAppServiceBase, IFinancialYearAppService
    {
        private readonly IRepository<FinancialYear> _financialYearRepository;
        private readonly IFinancialYearExcelExporter _financialYearExcelExporter;

        public FinancialYearAppService(IRepository<FinancialYear> financialYearRepository, IFinancialYearExcelExporter financialYearExcelExporter)
        {
            _financialYearRepository = financialYearRepository;
            _financialYearExcelExporter = financialYearExcelExporter;

        }

        public async Task<PagedResultDto<GetFinancialYearForViewDto>> GetAll(GetAllFinancialYearInput input)
        {

            var filteredFinancialYear = _financialYearRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.EffectiveFromDate.Contains(input.Filter) || e.EffectiveTillEndDate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EffectiveFromDateFilter), e => e.EffectiveFromDate.Contains(input.EffectiveFromDateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EffectiveTillEndDateFilter), e => e.EffectiveTillEndDate.Contains(input.EffectiveTillEndDateFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredFinancialYear = filteredFinancialYear
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var financialYear = from o in pagedAndFilteredFinancialYear
                                select new
                                {

                                    o.Name,
                                    o.Description,
                                    o.Code,
                                    o.EffectiveFromDate,
                                    o.EffectiveTillEndDate,
                                    o.IsActive,
                                    Id = o.Id
                                };

            var totalCount = await filteredFinancialYear.CountAsync();

            var dbList = await financialYear.ToListAsync();
            var results = new List<GetFinancialYearForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetFinancialYearForViewDto()
                {
                    FinancialYear = new FinancialYearDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Code = o.Code,
                        EffectiveFromDate = o.EffectiveFromDate,
                        EffectiveTillEndDate = o.EffectiveTillEndDate,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetFinancialYearForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetFinancialYearForViewDto> GetFinancialYearForView(int id)
        {
            var financialYear = await _financialYearRepository.GetAsync(id);

            var output = new GetFinancialYearForViewDto { FinancialYear = ObjectMapper.Map<FinancialYearDto>(financialYear) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FinancialYear_Edit)]
        public async Task<GetFinancialYearForEditOutput> GetFinancialYearForEdit(EntityDto input)
        {
            var financialYear = await _financialYearRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFinancialYearForEditOutput { FinancialYear = ObjectMapper.Map<CreateOrEditFinancialYearDto>(financialYear) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFinancialYearDto input)
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

        [AbpAuthorize(AppPermissions.Pages_FinancialYear_Create)]
        protected virtual async Task Create(CreateOrEditFinancialYearDto input)
        {
            var financialYear = ObjectMapper.Map<FinancialYear>(input);
            financialYear.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                financialYear.TenantId = (int?)AbpSession.TenantId;
            }

            await _financialYearRepository.InsertAsync(financialYear);

        }

        [AbpAuthorize(AppPermissions.Pages_FinancialYear_Edit)]
        protected virtual async Task Update(CreateOrEditFinancialYearDto input)
        {
            var financialYear = await _financialYearRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, financialYear);

        }

        [AbpAuthorize(AppPermissions.Pages_FinancialYear_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _financialYearRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFinancialYearToExcel(GetAllFinancialYearForExcelInput input)
        {

            var filteredFinancialYear = _financialYearRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.EffectiveFromDate.Contains(input.Filter) || e.EffectiveTillEndDate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EffectiveFromDateFilter), e => e.EffectiveFromDate.Contains(input.EffectiveFromDateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EffectiveTillEndDateFilter), e => e.EffectiveTillEndDate.Contains(input.EffectiveTillEndDateFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredFinancialYear
                         select new GetFinancialYearForViewDto()
                         {
                             FinancialYear = new FinancialYearDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 EffectiveFromDate = o.EffectiveFromDate,
                                 EffectiveTillEndDate = o.EffectiveTillEndDate,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var financialYearListDtos = await query.ToListAsync();

            return _financialYearExcelExporter.ExportToFile(financialYearListDtos);
        }

    }
}