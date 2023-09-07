using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IActivecurrencyAppService : IApplicationService
    {
        Task<PagedResultDto<GetActivecurrencyForViewDto>> GetAll(GetAllActivecurrencyInput input);

        Task<GetActivecurrencyForViewDto> GetActivecurrencyForView(int id);

        Task<GetActivecurrencyForEditOutput> GetActivecurrencyForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditActivecurrencyDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetActivecurrencyToExcel(GetAllActivecurrencyForExcelInput input);

    }
}