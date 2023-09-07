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
    [AbpAuthorize(AppPermissions.Pages_BusinessProcess)]
    public class BusinessProcessAppService : vitaAppServiceBase, IBusinessProcessAppService
    {
        private readonly IRepository<BusinessProcess> _businessProcessRepository;
        private readonly IBusinessProcessExcelExporter _businessProcessExcelExporter;

        public BusinessProcessAppService(IRepository<BusinessProcess> businessProcessRepository, IBusinessProcessExcelExporter businessProcessExcelExporter)
        {
            _businessProcessRepository = businessProcessRepository;
            _businessProcessExcelExporter = businessProcessExcelExporter;

        }

        public async Task<PagedResultDto<GetBusinessProcessForViewDto>> GetAll(GetAllBusinessProcessInput input)
        {

            var filteredBusinessProcess = _businessProcessRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredBusinessProcess = filteredBusinessProcess
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessProcess = from o in pagedAndFilteredBusinessProcess
                                  select new
                                  {

                                      o.Name,
                                      o.Description,
                                      o.Code,
                                      o.IsActive,
                                      Id = o.Id
                                  };

            var totalCount = await filteredBusinessProcess.CountAsync();

            var dbList = await businessProcess.ToListAsync();
            var results = new List<GetBusinessProcessForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessProcessForViewDto()
                {
                    BusinessProcess = new BusinessProcessDto
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

            return new PagedResultDto<GetBusinessProcessForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessProcessForViewDto> GetBusinessProcessForView(int id)
        {
            var businessProcess = await _businessProcessRepository.GetAsync(id);

            var output = new GetBusinessProcessForViewDto { BusinessProcess = ObjectMapper.Map<BusinessProcessDto>(businessProcess) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessProcess_Edit)]
        public async Task<GetBusinessProcessForEditOutput> GetBusinessProcessForEdit(EntityDto input)
        {
            var businessProcess = await _businessProcessRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessProcessForEditOutput { BusinessProcess = ObjectMapper.Map<CreateOrEditBusinessProcessDto>(businessProcess) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessProcessDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessProcess_Create)]
        protected virtual async Task Create(CreateOrEditBusinessProcessDto input)
        {
            var businessProcess = ObjectMapper.Map<BusinessProcess>(input);
            businessProcess.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                businessProcess.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessProcessRepository.InsertAsync(businessProcess);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessProcess_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessProcessDto input)
        {
            var businessProcess = await _businessProcessRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, businessProcess);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessProcess_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _businessProcessRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessProcessToExcel(GetAllBusinessProcessForExcelInput input)
        {

            var filteredBusinessProcess = _businessProcessRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredBusinessProcess
                         select new GetBusinessProcessForViewDto()
                         {
                             BusinessProcess = new BusinessProcessDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var businessProcessListDtos = await query.ToListAsync();

            return _businessProcessExcelExporter.ExportToFile(businessProcessListDtos);
        }

    }
}