using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNotePaymentDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNotePaymentDetailForViewDto>> GetAll(GetAllPurchaseDebitNotePaymentDetailInput input);

        Task<GetPurchaseDebitNotePaymentDetailForViewDto> GetPurchaseDebitNotePaymentDetailForView(long id);

        Task<GetPurchaseDebitNotePaymentDetailForEditOutput> GetPurchaseDebitNotePaymentDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseDebitNotePaymentDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}