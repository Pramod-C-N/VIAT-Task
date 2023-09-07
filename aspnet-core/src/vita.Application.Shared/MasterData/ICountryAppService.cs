using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface ICountryAppService : IApplicationService
    {
        Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountryInput input);

        Task<GetCountryForViewDto> GetCountryForView(int id);

        Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCountryDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCountryToExcel(GetAllCountryForExcelInput input);

    }
}