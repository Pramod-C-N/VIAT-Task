using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IBusinessOperationalModelAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessOperationalModelForViewDto>> GetAll(GetAllBusinessOperationalModelInput input);

        Task<GetBusinessOperationalModelForViewDto> GetBusinessOperationalModelForView(int id);

        Task<GetBusinessOperationalModelForEditOutput> GetBusinessOperationalModelForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditBusinessOperationalModelDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetBusinessOperationalModelToExcel(GetAllBusinessOperationalModelForExcelInput input);

    }
}