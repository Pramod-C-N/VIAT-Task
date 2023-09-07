using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.CustomReportSP.Exporting;
using vita.CustomReportSP.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.CustomReportSP
{
    [AbpAuthorize(AppPermissions.Pages_CustomReport)]
    public class CustomReportAppService : vitaAppServiceBase, ICustomReportAppService
    {
        private readonly IRepository<CustomReport> _customReportRepository;
        private readonly ICustomReportExcelExporter _customReportExcelExporter;

        public CustomReportAppService(IRepository<CustomReport> customReportRepository, ICustomReportExcelExporter customReportExcelExporter)
        {
            _customReportRepository = customReportRepository;
            _customReportExcelExporter = customReportExcelExporter;

        }

        public async Task<PagedResultDto<GetCustomReportForViewDto>> GetAll(GetAllCustomReportInput input)
        {

            var filteredCustomReport = _customReportRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReportName.Contains(input.Filter) || e.StoredProcedureName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReportNameFilter), e => e.ReportName.Contains(input.ReportNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoredProcedureNameFilter), e => e.StoredProcedureName.Contains(input.StoredProcedureNameFilter));

            var pagedAndFilteredCustomReport = filteredCustomReport
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customReport = from o in pagedAndFilteredCustomReport
                               select new
                               {

                                   o.ReportName,
                                   o.StoredProcedureName,
                                   Id = o.Id
                               };

            var totalCount = await filteredCustomReport.CountAsync();

            var dbList = await customReport.ToListAsync();
            var results = new List<GetCustomReportForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomReportForViewDto()
                {
                    CustomReport = new CustomReportDto
                    {

                        ReportName = o.ReportName,
                        StoredProcedureName = o.StoredProcedureName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomReportForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCustomReportForViewDto> GetCustomReportForView(int id)
        {
            var customReport = await _customReportRepository.GetAsync(id);

            var output = new GetCustomReportForViewDto { CustomReport = ObjectMapper.Map<CustomReportDto>(customReport) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CustomReport_Edit)]
        public async Task<GetCustomReportForEditOutput> GetCustomReportForEdit(EntityDto input)
        {
            var customReport = await _customReportRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomReportForEditOutput { CustomReport = ObjectMapper.Map<CreateOrEditCustomReportDto>(customReport) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomReportDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomReport_Create)]
        protected virtual async Task Create(CreateOrEditCustomReportDto input)
        {
            var customReport = ObjectMapper.Map<CustomReport>(input);
            customReport.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                customReport.TenantId = (int?)AbpSession.TenantId;
            }

            await _customReportRepository.InsertAsync(customReport);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomReport_Edit)]
        protected virtual async Task Update(CreateOrEditCustomReportDto input)
        {
            var customReport = await _customReportRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, customReport);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomReport_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _customReportRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCustomReportToExcel(GetAllCustomReportForExcelInput input)
        {

            var filteredCustomReport = _customReportRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReportName.Contains(input.Filter) || e.StoredProcedureName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReportNameFilter), e => e.ReportName.Contains(input.ReportNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoredProcedureNameFilter), e => e.StoredProcedureName.Contains(input.StoredProcedureNameFilter));

            var query = (from o in filteredCustomReport
                         select new GetCustomReportForViewDto()
                         {
                             CustomReport = new CustomReportDto
                             {
                                 ReportName = o.ReportName,
                                 StoredProcedureName = o.StoredProcedureName,
                                 Id = o.Id
                             }
                         });

            var customReportListDtos = await query.ToListAsync();

            return _customReportExcelExporter.ExportToFile(customReportListDtos);
        }

    }
}