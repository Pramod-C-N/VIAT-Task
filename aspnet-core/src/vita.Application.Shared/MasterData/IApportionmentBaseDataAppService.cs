using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IApportionmentBaseDataAppService : IApplicationService
    {
        Task<PagedResultDto<GetApportionmentBaseDataForViewDto>> GetAll(GetAllApportionmentBaseDataInput input);

        Task<GetApportionmentBaseDataForViewDto> GetApportionmentBaseDataForView(int id);

        Task<GetApportionmentBaseDataForEditOutput> GetApportionmentBaseDataForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditApportionmentBaseDataDto input);

        Task Delete(EntityDto input);

    }
}