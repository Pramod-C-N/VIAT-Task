using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;

namespace vita.DraftFee
{
    public interface IDraftPartiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDraftPartyForViewDto>> GetAll(GetAllDraftPartiesInput input);

        Task<GetDraftPartyForViewDto> GetDraftPartyForView(long id);

        Task<GetDraftPartyForEditOutput> GetDraftPartyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDraftPartyDto input);

        Task Delete(EntityDto<long> input);

    }
}