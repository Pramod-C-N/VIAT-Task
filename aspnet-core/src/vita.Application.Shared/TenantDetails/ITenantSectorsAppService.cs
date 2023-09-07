using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantSectorsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantSectorsForViewDto>> GetAll(GetAllTenantSectorsInput input);

        Task<GetTenantSectorsForEditOutput> GetTenantSectorsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantSectorsDto input);

        Task Delete(EntityDto input);

    }
}