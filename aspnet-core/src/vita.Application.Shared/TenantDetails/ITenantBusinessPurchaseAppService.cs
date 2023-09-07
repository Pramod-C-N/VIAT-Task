using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantBusinessPurchaseAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantBusinessPurchaseForViewDto>> GetAll(GetAllTenantBusinessPurchaseInput input);

        Task<GetTenantBusinessPurchaseForEditOutput> GetTenantBusinessPurchaseForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantBusinessPurchaseDto input);

        Task Delete(EntityDto input);

    }
}