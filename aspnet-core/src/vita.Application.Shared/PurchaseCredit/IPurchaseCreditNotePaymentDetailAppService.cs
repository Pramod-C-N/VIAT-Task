using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNotePaymentDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNotePaymentDetailForViewDto>> GetAll(GetAllPurchaseCreditNotePaymentDetailInput input);

        Task<GetPurchaseCreditNotePaymentDetailForViewDto> GetPurchaseCreditNotePaymentDetailForView(long id);

        Task<GetPurchaseCreditNotePaymentDetailForEditOutput> GetPurchaseCreditNotePaymentDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseCreditNotePaymentDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}