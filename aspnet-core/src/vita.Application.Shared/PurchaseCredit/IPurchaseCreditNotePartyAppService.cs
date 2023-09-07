using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNotePartyAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNotePartyForViewDto>> GetAll(GetAllPurchaseCreditNotePartyInput input);

        Task<GetPurchaseCreditNotePartyForViewDto> GetPurchaseCreditNotePartyForView(long id);

        Task<GetPurchaseCreditNotePartyForEditOutput> GetPurchaseCreditNotePartyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseCreditNotePartyDto input);

        Task Delete(EntityDto<long> input);

    }
}