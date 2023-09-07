using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;

namespace vita.DraftFee
{
    public interface IDraftDiscountsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDraftDiscountForViewDto>> GetAll(GetAllDraftDiscountsInput input);

        Task<GetDraftDiscountForViewDto> GetDraftDiscountForView(long id);

        Task<GetDraftDiscountForEditOutput> GetDraftDiscountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDraftDiscountDto input);

        Task Delete(EntityDto<long> input);

    }
}