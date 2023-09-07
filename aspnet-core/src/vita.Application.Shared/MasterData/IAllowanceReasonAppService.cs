using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IAllowanceReasonAppService : IApplicationService
    {
        Task<PagedResultDto<GetAllowanceReasonForViewDto>> GetAll(GetAllAllowanceReasonInput input);

        Task<GetAllowanceReasonForViewDto> GetAllowanceReasonForView(int id);

        Task<GetAllowanceReasonForEditOutput> GetAllowanceReasonForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAllowanceReasonDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAllowanceReasonToExcel(GetAllAllowanceReasonForExcelInput input);

    }
}