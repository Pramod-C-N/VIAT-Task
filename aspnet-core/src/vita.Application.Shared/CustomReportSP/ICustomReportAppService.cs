using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.CustomReportSP.Dtos;
using vita.Dto;

namespace vita.CustomReportSP
{
    public interface ICustomReportAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomReportForViewDto>> GetAll(GetAllCustomReportInput input);

        Task<GetCustomReportForViewDto> GetCustomReportForView(int id);

        Task<GetCustomReportForEditOutput> GetCustomReportForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCustomReportDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCustomReportToExcel(GetAllCustomReportForExcelInput input);

    }
}