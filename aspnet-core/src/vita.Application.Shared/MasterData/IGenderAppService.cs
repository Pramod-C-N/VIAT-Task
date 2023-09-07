using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IGenderAppService : IApplicationService
    {
        Task<PagedResultDto<GetGenderForViewDto>> GetAll(GetAllGenderInput input);

        Task<GetGenderForViewDto> GetGenderForView(int id);

        Task<GetGenderForEditOutput> GetGenderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditGenderDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetGenderToExcel(GetAllGenderForExcelInput input);

    }
}