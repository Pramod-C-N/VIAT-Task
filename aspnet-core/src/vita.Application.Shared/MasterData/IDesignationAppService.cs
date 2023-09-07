using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IDesignationAppService : IApplicationService
    {
        Task<PagedResultDto<GetDesignationForViewDto>> GetAll(GetAllDesignationInput input);

        Task<GetDesignationForViewDto> GetDesignationForView(int id);

        Task<GetDesignationForEditOutput> GetDesignationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDesignationDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetDesignationToExcel(GetAllDesignationForExcelInput input);

    }
}