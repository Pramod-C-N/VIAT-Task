using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;

namespace vita.DraftFee
{
    public interface IDraftContactPersonsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDraftContactPersonForViewDto>> GetAll(GetAllDraftContactPersonsInput input);

        Task<GetDraftContactPersonForViewDto> GetDraftContactPersonForView(long id);

        Task<GetDraftContactPersonForEditOutput> GetDraftContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDraftContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}