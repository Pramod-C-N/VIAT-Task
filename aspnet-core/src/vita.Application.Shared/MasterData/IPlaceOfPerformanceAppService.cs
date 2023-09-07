using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IPlaceOfPerformanceAppService : IApplicationService
    {
        Task<PagedResultDto<GetPlaceOfPerformanceForViewDto>> GetAll(GetAllPlaceOfPerformanceInput input);

        Task<GetPlaceOfPerformanceForViewDto> GetPlaceOfPerformanceForView(int id);

        Task<GetPlaceOfPerformanceForEditOutput> GetPlaceOfPerformanceForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPlaceOfPerformanceDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetPlaceOfPerformanceToExcel(GetAllPlaceOfPerformanceForExcelInput input);

    }
}