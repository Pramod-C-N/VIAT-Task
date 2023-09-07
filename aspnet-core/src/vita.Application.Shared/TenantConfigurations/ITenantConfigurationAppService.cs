using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantConfigurations.Dtos;
using vita.Dto;

namespace vita.TenantConfigurations
{
    public interface ITenantConfigurationAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantConfigurationForViewDto>> GetAll(GetAllTenantConfigurationInput input);

        Task<GetTenantConfigurationForEditOutput> GetTenantConfigurationByTransactionType(string transType);

        Task CreateOrEdit(CreateOrEditTenantConfigurationDto input);

        Task Delete(EntityDto<long> input);

    }
}