using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface ISectorAppService : IApplicationService
    {
        Task<PagedResultDto<GetSectorForViewDto>> GetAll(GetAllSectorInput input);

        Task<GetSectorForViewDto> GetSectorForView(int id);

        Task<GetSectorForEditOutput> GetSectorForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditSectorDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSectorToExcel(GetAllSectorForExcelInput input);

    }
}