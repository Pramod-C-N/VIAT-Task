using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;

namespace vita.DraftFee
{
    public interface IDraftSummariesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDraftSummaryForViewDto>> GetAll(GetAllDraftSummariesInput input);

        Task<GetDraftSummaryForViewDto> GetDraftSummaryForView(long id);

        Task<GetDraftSummaryForEditOutput> GetDraftSummaryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDraftSummaryDto input);

        Task Delete(EntityDto<long> input);

    }
}