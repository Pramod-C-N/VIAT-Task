using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;

namespace vita.DraftFee
{
    public interface IDraftPaymentDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDraftPaymentDetailForViewDto>> GetAll(GetAllDraftPaymentDetailsInput input);

        Task<GetDraftPaymentDetailForViewDto> GetDraftPaymentDetailForView(long id);

        Task<GetDraftPaymentDetailForEditOutput> GetDraftPaymentDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDraftPaymentDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}