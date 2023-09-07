using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;

namespace vita.DraftFee
{
    public interface IDraftVATDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDraftVATDetailForViewDto>> GetAll(GetAllDraftVATDetailsInput input);

        Task<GetDraftVATDetailForEditOutput> GetDraftVATDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDraftVATDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}