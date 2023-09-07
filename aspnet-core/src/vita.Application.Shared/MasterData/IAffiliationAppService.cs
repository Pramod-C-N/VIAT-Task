using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IAffiliationAppService : IApplicationService
    {
        Task<PagedResultDto<GetAffiliationForViewDto>> GetAll(GetAllAffiliationInput input);

        Task<GetAffiliationForViewDto> GetAffiliationForView(int id);

        Task<GetAffiliationForEditOutput> GetAffiliationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAffiliationDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAffiliationToExcel(GetAllAffiliationForExcelInput input);

    }
}