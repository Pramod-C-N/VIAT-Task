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
    [AbpAuthorize(AppPermissions.Pages_ExemptionReason)]
    public class ExemptionReasonAppService : vitaAppServiceBase, IExemptionReasonAppService
    {
        private readonly IRepository<ExemptionReason> _exemptionReasonRepository;
        private readonly IExemptionReasonExcelExporter _exemptionReasonExcelExporter;

        public ExemptionReasonAppService(IRepository<ExemptionReason> exemptionReasonRepository, IExemptionReasonExcelExporter exemptionReasonExcelExporter)
        {
            _exemptionReasonRepository = exemptionReasonRepository;
            _exemptionReasonExcelExporter = exemptionReasonExcelExporter;

        }

        public async Task<PagedResultDto<GetExemptionReasonForViewDto>> GetAll(GetAllExemptionReasonInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {

                var filteredExemptionReason = _exemptionReasonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredExemptionReason = filteredExemptionReason
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var exemptionReason = from o in pagedAndFilteredExemptionReason
                                      select new
                                      {

                                          o.Name,
                                          o.Description,
                                          o.Code,
                                          o.IsActive,
                                          Id = o.Id
                                      };

                var totalCount = await filteredExemptionReason.CountAsync();

                var dbList = await exemptionReason.ToListAsync();
                var results = new List<GetExemptionReasonForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetExemptionReasonForViewDto()
                    {
                        ExemptionReason = new ExemptionReasonDto
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

                return new PagedResultDto<GetExemptionReasonForViewDto>(
                    totalCount,
                    results
                );

            }
        }
        public async Task<GetExemptionReasonForViewDto> GetExemptionReasonForView(int id)
        {
            var exemptionReason = await _exemptionReasonRepository.GetAsync(id);

            var output = new GetExemptionReasonForViewDto { ExemptionReason = ObjectMapper.Map<ExemptionReasonDto>(exemptionReason) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ExemptionReason_Edit)]
        public async Task<GetExemptionReasonForEditOutput> GetExemptionReasonForEdit(EntityDto input)
        {
            var exemptionReason = await _exemptionReasonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetExemptionReasonForEditOutput { ExemptionReason = ObjectMapper.Map<CreateOrEditExemptionReasonDto>(exemptionReason) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditExemptionReasonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ExemptionReason_Create)]
        protected virtual async Task Create(CreateOrEditExemptionReasonDto input)
        {
            var exemptionReason = ObjectMapper.Map<ExemptionReason>(input);
            exemptionReason.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                exemptionReason.TenantId = (int?)AbpSession.TenantId;
            }

            await _exemptionReasonRepository.InsertAsync(exemptionReason);

        }

        [AbpAuthorize(AppPermissions.Pages_ExemptionReason_Edit)]
        protected virtual async Task Update(CreateOrEditExemptionReasonDto input)
        {
            var exemptionReason = await _exemptionReasonRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, exemptionReason);

        }

        [AbpAuthorize(AppPermissions.Pages_ExemptionReason_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _exemptionReasonRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetExemptionReasonToExcel(GetAllExemptionReasonForExcelInput input)
        {

            var filteredExemptionReason = _exemptionReasonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredExemptionReason
                         select new GetExemptionReasonForViewDto()
                         {
                             ExemptionReason = new ExemptionReasonDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var exemptionReasonListDtos = await query.ToListAsync();

            return _exemptionReasonExcelExporter.ExportToFile(exemptionReasonListDtos);
        }

    }
}