using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IErrorGroupAppService : IApplicationService
    {
        Task<PagedResultDto<GetErrorGroupForViewDto>> GetAll(GetAllErrorGroupInput input);

        Task<GetErrorGroupForViewDto> GetErrorGroupForView(int id);

        Task<GetErrorGroupForEditOutput> GetErrorGroupForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditErrorGroupDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetErrorGroupToExcel(GetAllErrorGroupForExcelInput input);

    }
}