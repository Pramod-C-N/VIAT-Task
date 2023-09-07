using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNoteItemAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNoteItemForViewDto>> GetAll(GetAllPurchaseCreditNoteItemInput input);

        Task<GetPurchaseCreditNoteItemForViewDto> GetPurchaseCreditNoteItemForView(long id);

        Task<GetPurchaseCreditNoteItemForEditOutput> GetPurchaseCreditNoteItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseCreditNoteItemDto input);

        Task Delete(EntityDto<long> input);

    }
}