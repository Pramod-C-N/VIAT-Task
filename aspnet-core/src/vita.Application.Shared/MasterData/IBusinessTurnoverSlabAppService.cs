using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IBusinessTurnoverSlabAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessTurnoverSlabForViewDto>> GetAll(GetAllBusinessTurnoverSlabInput input);

        Task<GetBusinessTurnoverSlabForViewDto> GetBusinessTurnoverSlabForView(int id);

        Task<GetBusinessTurnoverSlabForEditOutput> GetBusinessTurnoverSlabForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditBusinessTurnoverSlabDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetBusinessTurnoverSlabToExcel(GetAllBusinessTurnoverSlabForExcelInput input);

    }
}