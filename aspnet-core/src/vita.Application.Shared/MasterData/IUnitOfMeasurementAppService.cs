using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IUnitOfMeasurementAppService : IApplicationService
    {
        Task<PagedResultDto<GetUnitOfMeasurementForViewDto>> GetAll(GetAllUnitOfMeasurementInput input);

        Task<GetUnitOfMeasurementForViewDto> GetUnitOfMeasurementForView(int id);

        Task<GetUnitOfMeasurementForEditOutput> GetUnitOfMeasurementForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditUnitOfMeasurementDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetUnitOfMeasurementToExcel(GetAllUnitOfMeasurementForExcelInput input);

    }
}