using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.MasterData.Dtos;
using vita.Dto;

namespace vita.MasterData
{
    public interface IIRNMastersAppService : IApplicationService
    {
        Task<PagedResultDto<GetIRNMasterForViewDto>> GetAll(GetAllIRNMastersInput input);

        Task<GetIRNMasterForViewDto> GetIRNMasterForView(long id);

        Task<GetIRNMasterForEditOutput> GetIRNMasterForEdit(EntityDto<long> input);

        Task<TransactionDto> CreateOrEdit(CreateOrEditIRNMasterDto input);

        Task Delete(EntityDto<long> input);

    }
}