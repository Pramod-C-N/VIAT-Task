using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.TenantConfigurations.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.TenantConfigurations
{
    [AbpAuthorize(AppPermissions.Pages_TenantConfiguration)]
    public class TenantConfigurationAppService : vitaAppServiceBase, ITenantConfigurationAppService
    {
        private readonly IRepository<TenantConfiguration, long> _tenantConfigurationRepository;

        public TenantConfigurationAppService(IRepository<TenantConfiguration, long> tenantConfigurationRepository)
        {
            _tenantConfigurationRepository = tenantConfigurationRepository;

        }

        public async Task<PagedResultDto<GetTenantConfigurationForViewDto>> GetAll(GetAllTenantConfigurationInput input)
        {

            var filteredTenantConfiguration = _tenantConfigurationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TransactionType.Contains(input.Filter) || e.ShipmentJson.Contains(input.Filter) || e.AdditionalFieldsJson.Contains(input.Filter) || e.EmailJson.Contains(input.Filter) || e.AdditionalData1.Contains(input.Filter) || e.AdditionalData2.Contains(input.Filter) || e.AdditionalData3.Contains(input.Filter));

            var pagedAndFilteredTenantConfiguration = filteredTenantConfiguration
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantConfiguration = from o in pagedAndFilteredTenantConfiguration
                                      select new
                                      {
                                          Id = o.Id,
                                          isPhase1 = o.isPhase1,
                                          language = o.Language,
                                          TransactionType = o.TransactionType,
                                          ShipmentJson = o.ShipmentJson,
                                          AdditionalFieldsJson = o.AdditionalFieldsJson,
                                          EmailJson = o.EmailJson,
                                          AdditionalData1 = o.AdditionalData1,
                                          AdditionalData2 = o.AdditionalData2,
                                          AdditionalData3 = o.AdditionalData3,
                                          isActive = o.isActive
                                      };

            var totalCount = await filteredTenantConfiguration.CountAsync();

            var dbList = await tenantConfiguration.ToListAsync();
            var results = new List<GetTenantConfigurationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantConfigurationForViewDto()
                {
                    TenantConfiguration = new TenantConfigurationDto
                    {

                        Id = o.Id,
                        isPhase1 = o.isPhase1,
                        Language = o.language,
                        TransactionType = o.TransactionType,
                        ShipmentJson = o.ShipmentJson,
                        AdditionalFieldsJson = o.AdditionalFieldsJson,
                        EmailJson = o.EmailJson,
                        AdditionalData1 = o.AdditionalData1,
                        AdditionalData2 = o.AdditionalData2,
                        AdditionalData3 = o.AdditionalData3,
                        isActive = o.isActive
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantConfigurationForViewDto>(
                totalCount,
                results
            );

        }


        public async Task<GetTenantConfigurationForEditOutput> GetTenantConfigurationByTransactionType(string transType)
        {
            var tenantConfiguration = await _tenantConfigurationRepository.FirstOrDefaultAsync(a => a.TenantId == AbpSession.TenantId && a.TransactionType == transType && a.isActive == true);

            var output = new GetTenantConfigurationForEditOutput { TenantConfiguration = ObjectMapper.Map<CreateOrEditTenantConfigurationDto>(tenantConfiguration) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantConfigurationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantConfiguration_Create)]
        protected virtual async Task Create(CreateOrEditTenantConfigurationDto input)
        {
            var tenantConfiguration = ObjectMapper.Map<TenantConfiguration>(input);

            if (AbpSession.TenantId != null)
            {
                tenantConfiguration.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantConfigurationRepository.InsertAsync(tenantConfiguration);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantConfiguration_Edit)]
        protected virtual async Task Update(CreateOrEditTenantConfigurationDto input)
        {

            var tenantConfiguration = await _tenantConfigurationRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, tenantConfiguration);


        }

        [AbpAuthorize(AppPermissions.Pages_TenantConfiguration_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _tenantConfigurationRepository.DeleteAsync(input.Id);
        }

    }
}