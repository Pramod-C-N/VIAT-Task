using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantPurchaseVatCateoryAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantPurchaseVatCateoryForViewDto>> GetAll(GetAllTenantPurchaseVatCateoryInput input);

        Task<GetTenantPurchaseVatCateoryForEditOutput> GetTenantPurchaseVatCateoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantPurchaseVatCateoryDto input);

        Task Delete(EntityDto input);

    }
}