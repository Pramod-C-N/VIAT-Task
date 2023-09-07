using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
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
    [AbpAuthorize(AppPermissions.Pages_Administration_Module)]
    public class ModuleAppService : vitaAppServiceBase, IModuleAppService
    {
        private readonly IRepository<Module, long> _moduleRepository;

        public ModuleAppService(IRepository<Module, long> moduleRepository)
        {
            _moduleRepository = moduleRepository;

        }

        public async Task<PagedResultDto<GetModuleForViewDto>> GetAll(GetAllModuleInput input)
        {

            var filteredModule = _moduleRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ModuleName.Contains(input.Filter))
                        .WhereIf(input.MinModuleIdFilter != null, e => e.ModuleId >= input.MinModuleIdFilter)
                        .WhereIf(input.MaxModuleIdFilter != null, e => e.ModuleId <= input.MaxModuleIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNameFilter), e => e.ModuleName.Contains(input.ModuleNameFilter))
                        .WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
                        .WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
                        .WhereIf(input.MinTenantIdFilter != null, e => e.TenantId >= input.MinTenantIdFilter)
                        .WhereIf(input.MaxTenantIdFilter != null, e => e.TenantId <= input.MaxTenantIdFilter);

            var pagedAndFilteredModule = filteredModule
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var module = from o in pagedAndFilteredModule
                         select new
                         {

                             o.ModuleId,
                             o.ModuleName,
                             o.Status,
                             o.TenantId,
                             Id = o.Id
                         };

            var totalCount = await filteredModule.CountAsync();

            var dbList = await module.ToListAsync();
            var results = new List<GetModuleForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetModuleForViewDto()
                {
                    Module = new ModuleDto
                    {

                        ModuleId = o.ModuleId,
                        ModuleName = o.ModuleName,
                        Status = o.Status,
                        TenantId = o.TenantId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetModuleForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetModuleForViewDto> GetModuleForView(long id)
        {
            var module = await _moduleRepository.GetAsync(id);

            var output = new GetModuleForViewDto { Module = ObjectMapper.Map<ModuleDto>(module) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Module_Edit)]
        public async Task<GetModuleForEditOutput> GetModuleForEdit(EntityDto<long> input)
        {
            var module = await _moduleRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetModuleForEditOutput { Module = ObjectMapper.Map<CreateOrEditModuleDto>(module) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditModuleDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Module_Create)]
        protected virtual async Task Create(CreateOrEditModuleDto input)
        {
            var module = ObjectMapper.Map<Module>(input);

            await _moduleRepository.InsertAsync(module);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Module_Edit)]
        protected virtual async Task Update(CreateOrEditModuleDto input)
        {
            var module = await _moduleRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, module);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Module_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _moduleRepository.DeleteAsync(input.Id);
        }

    }
}