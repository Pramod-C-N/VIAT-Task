using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IErrorTypeAppService : IApplicationService
    {
        Task<PagedResultDto<GetErrorTypeForViewDto>> GetAll(GetAllErrorTypeInput input);

        Task<GetErrorTypeForViewDto> GetErrorTypeForView(int id);

        Task<GetErrorTypeForEditOutput> GetErrorTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditErrorTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetErrorTypeToExcel(GetAllErrorTypeForExcelInput input);

    }
}