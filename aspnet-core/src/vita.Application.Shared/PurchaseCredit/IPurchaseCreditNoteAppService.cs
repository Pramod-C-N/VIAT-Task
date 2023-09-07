using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNoteAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNoteForViewDto>> GetAll(GetAllPurchaseCreditNoteInput input);

        Task<GetPurchaseCreditNoteForViewDto> GetPurchaseCreditNoteForView(long id);

        Task<GetPurchaseCreditNoteForEditOutput> GetPurchaseCreditNoteForEdit(EntityDto<long> input);

        Task<long> CreateOrEdit(CreateOrEditPurchaseCreditNoteDto input);

        Task Delete(EntityDto<long> input);


    }
}