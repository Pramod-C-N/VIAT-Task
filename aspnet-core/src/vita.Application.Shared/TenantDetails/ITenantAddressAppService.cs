using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantAddressAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantAddressForViewDto>> GetAll(GetAllTenantAddressInput input);

        Task<GetTenantAddressForViewDto> GetTenantAddressForView(int id);

        Task<GetTenantAddressForEditOutput> GetTenantAddressForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantAddressDto input);

        Task Delete(EntityDto input);

    }
}