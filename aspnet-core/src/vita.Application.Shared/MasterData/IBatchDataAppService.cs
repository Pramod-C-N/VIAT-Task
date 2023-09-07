using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IBatchDataAppService : IApplicationService
    {
        Task<PagedResultDto<GetBatchDataForViewDto>> GetAll(GetAllBatchDataInput input);

        Task<GetBatchDataForEditOutput> GetBatchDataForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBatchDataDto input);

        Task Delete(EntityDto<long> input);

    }
}