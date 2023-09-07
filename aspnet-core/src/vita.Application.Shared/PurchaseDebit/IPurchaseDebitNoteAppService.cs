using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNoteAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNoteForViewDto>> GetAll(GetAllPurchaseDebitNoteInput input);

        Task<GetPurchaseDebitNoteForViewDto> GetPurchaseDebitNoteForView(long id);

        Task<GetPurchaseDebitNoteForEditOutput> GetPurchaseDebitNoteForEdit(EntityDto<long> input);

        Task<long> CreateOrEdit(CreateOrEditPurchaseDebitNoteDto input);

        Task Delete(EntityDto<long> input);

    }
}