using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface ICurrencyAppService : IApplicationService
    {
        Task<PagedResultDto<GetCurrencyForViewDto>> GetAll(GetAllCurrencyInput input);

        Task<GetCurrencyForViewDto> GetCurrencyForView(int id);

        Task<GetCurrencyForEditOutput> GetCurrencyForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCurrencyDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCurrencyToExcel(GetAllCurrencyForExcelInput input);

    }
}