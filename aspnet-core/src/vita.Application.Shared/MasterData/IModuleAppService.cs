using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IModuleAppService : IApplicationService
    {
        Task<PagedResultDto<GetModuleForViewDto>> GetAll(GetAllModuleInput input);

        Task<GetModuleForViewDto> GetModuleForView(long id);

        Task<GetModuleForEditOutput> GetModuleForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditModuleDto input);

        Task Delete(EntityDto<long> input);

    }
}