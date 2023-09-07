using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IOrganisationTypeAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrganisationTypeForViewDto>> GetAll(GetAllOrganisationTypeInput input);

        Task<GetOrganisationTypeForViewDto> GetOrganisationTypeForView(int id);

        Task<GetOrganisationTypeForEditOutput> GetOrganisationTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOrganisationTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOrganisationTypeToExcel(GetAllOrganisationTypeForExcelInput input);

    }
}