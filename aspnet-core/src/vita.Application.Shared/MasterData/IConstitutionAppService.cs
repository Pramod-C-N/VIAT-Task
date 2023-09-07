using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IConstitutionAppService : IApplicationService
    {
        Task<PagedResultDto<GetConstitutionForViewDto>> GetAll(GetAllConstitutionInput input);

        Task<GetConstitutionForViewDto> GetConstitutionForView(int id);

        Task<GetConstitutionForEditOutput> GetConstitutionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditConstitutionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetConstitutionToExcel(GetAllConstitutionForExcelInput input);

    }
}