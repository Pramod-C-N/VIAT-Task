using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantSupplyVATCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantSupplyVATCategoryForViewDto>> GetAll(GetAllTenantSupplyVATCategoryInput input);

        Task<GetTenantSupplyVATCategoryForEditOutput> GetTenantSupplyVATCategoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantSupplyVATCategoryDto input);

        Task Delete(EntityDto input);

    }
}