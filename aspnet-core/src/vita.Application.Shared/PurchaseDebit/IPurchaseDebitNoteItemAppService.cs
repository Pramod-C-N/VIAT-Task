using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNoteItemAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNoteItemForViewDto>> GetAll(GetAllPurchaseDebitNoteItemInput input);

        Task<GetPurchaseDebitNoteItemForViewDto> GetPurchaseDebitNoteItemForView(long id);

        Task<GetPurchaseDebitNoteItemForEditOutput> GetPurchaseDebitNoteItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseDebitNoteItemDto input);

        Task Delete(EntityDto<long> input);

    }
}