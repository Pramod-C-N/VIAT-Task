using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface ITitleAppService : IApplicationService
    {
        Task<PagedResultDto<GetTitleForViewDto>> GetAll(GetAllTitleInput input);

        Task<GetTitleForViewDto> GetTitleForView(int id);

        Task<GetTitleForEditOutput> GetTitleForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTitleDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTitleToExcel(GetAllTitleForExcelInput input);

    }
}