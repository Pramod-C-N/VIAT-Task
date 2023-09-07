using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantBankDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantBankDetailForViewDto>> GetAll(GetAllTenantBankDetailsInput input);

        Task<GetTenantBankDetailForEditOutput> GetTenantBankDetailForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantBankDetailDto input);

        Task Delete(EntityDto input);

    }
}