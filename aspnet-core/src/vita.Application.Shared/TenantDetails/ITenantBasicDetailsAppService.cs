using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.TenantDetails.Dtos;
using vita.Dto;
using System.Data;

namespace vita.TenantDetails
{
    public interface ITenantBasicDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantBasicDetailsForViewDto>> GetAll(GetAllTenantBasicDetailsInput input);

        Task<GetTenantBasicDetailsForViewDto> GetTenantBasicDetailsForView(int id);

        Task<GetTenantBasicDetailsForEditOutput> GetTenantBasicDetailsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantBasicDetailsDto input);

        Task Delete(EntityDto input);
        Task<bool> InsertBatchUploadTenant(string json, string fileName, int? tenantId);

        Task<DataTable> GetTenantById(int? Id);

    }
}