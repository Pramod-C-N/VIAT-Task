using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNoteDiscountAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNoteDiscountForViewDto>> GetAll(GetAllPurchaseDebitNoteDiscountInput input);

        Task<GetPurchaseDebitNoteDiscountForViewDto> GetPurchaseDebitNoteDiscountForView(long id);

        Task<GetPurchaseDebitNoteDiscountForEditOutput> GetPurchaseDebitNoteDiscountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseDebitNoteDiscountDto input);

        Task Delete(EntityDto<long> input);

    }
}