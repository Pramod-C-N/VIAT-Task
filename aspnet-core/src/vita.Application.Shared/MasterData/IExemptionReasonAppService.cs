using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IExemptionReasonAppService : IApplicationService
    {
        Task<PagedResultDto<GetExemptionReasonForViewDto>> GetAll(GetAllExemptionReasonInput input);

        Task<GetExemptionReasonForViewDto> GetExemptionReasonForView(int id);

        Task<GetExemptionReasonForEditOutput> GetExemptionReasonForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditExemptionReasonDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetExemptionReasonToExcel(GetAllExemptionReasonForExcelInput input);

    }
}