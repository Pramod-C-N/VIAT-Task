using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;

namespace vita.TenantDetails
{
    public interface ITenantDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantDocumentsForViewDto>> GetAll(GetAllTenantDocumentsInput input);

        Task<GetTenantDocumentsForEditOutput> GetTenantDocumentsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantDocumentsDto input);

        Task Delete(EntityDto input);

    }
}