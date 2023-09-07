using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNoteDiscountAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNoteDiscountForViewDto>> GetAll(GetAllPurchaseCreditNoteDiscountInput input);

        Task<GetPurchaseCreditNoteDiscountForViewDto> GetPurchaseCreditNoteDiscountForView(long id);

        Task<GetPurchaseCreditNoteDiscountForEditOutput> GetPurchaseCreditNoteDiscountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseCreditNoteDiscountDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetPurchaseCreditNoteDiscountToExcel(GetAllPurchaseCreditNoteDiscountForExcelInput input);

    }
}