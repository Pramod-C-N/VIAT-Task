using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface INatureofServicesAppService : IApplicationService
    {
        Task<PagedResultDto<GetNatureofServicesForViewDto>> GetAll(GetAllNatureofServicesInput input);

        Task<GetNatureofServicesForViewDto> GetNatureofServicesForView(int id);

        Task<GetNatureofServicesForEditOutput> GetNatureofServicesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditNatureofServicesDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetNatureofServicesToExcel(GetAllNatureofServicesForExcelInput input);

    }
}