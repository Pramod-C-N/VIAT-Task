using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNoteVATDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNoteVATDetailForViewDto>> GetAll(GetAllPurchaseCreditNoteVATDetailInput input);

        Task<GetPurchaseCreditNoteVATDetailForViewDto> GetPurchaseCreditNoteVATDetailForView(long id);

        Task<GetPurchaseCreditNoteVATDetailForEditOutput> GetPurchaseCreditNoteVATDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseCreditNoteVATDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}