using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IBusinessProcessAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessProcessForViewDto>> GetAll(GetAllBusinessProcessInput input);

        Task<GetBusinessProcessForViewDto> GetBusinessProcessForView(int id);

        Task<GetBusinessProcessForEditOutput> GetBusinessProcessForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditBusinessProcessDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetBusinessProcessToExcel(GetAllBusinessProcessForExcelInput input);

    }
}