using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantShareHoldersAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantShareHoldersForViewDto>> GetAll(GetAllTenantShareHoldersInput input);

        Task<GetTenantShareHoldersForViewDto> GetTenantShareHoldersForView(int id);

        Task<GetTenantShareHoldersForEditOutput> GetTenantShareHoldersForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantShareHoldersDto input);

        Task Delete(EntityDto input);

    }
}