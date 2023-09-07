using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface ITenantTypeAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantTypeForViewDto>> GetAll(GetAllTenantTypeInput input);

        Task<GetTenantTypeForViewDto> GetTenantTypeForView(int id);

        Task<GetTenantTypeForEditOutput> GetTenantTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTenantTypeToExcel(GetAllTenantTypeForExcelInput input);

    }
}