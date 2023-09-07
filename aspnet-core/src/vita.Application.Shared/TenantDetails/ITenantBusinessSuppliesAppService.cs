using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantBusinessSuppliesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantBusinessSuppliesForViewDto>> GetAll(GetAllTenantBusinessSuppliesInput input);

        Task<GetTenantBusinessSuppliesForEditOutput> GetTenantBusinessSuppliesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantBusinessSuppliesDto input);

        Task Delete(EntityDto input);

    }
}