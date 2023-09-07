using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IFinancialYearAppService : IApplicationService
    {
        Task<PagedResultDto<GetFinancialYearForViewDto>> GetAll(GetAllFinancialYearInput input);

        Task<GetFinancialYearForViewDto> GetFinancialYearForView(int id);

        Task<GetFinancialYearForEditOutput> GetFinancialYearForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFinancialYearDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFinancialYearToExcel(GetAllFinancialYearForExcelInput input);

    }
}