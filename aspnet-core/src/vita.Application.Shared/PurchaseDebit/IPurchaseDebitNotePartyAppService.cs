using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNotePartyAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNotePartyForViewDto>> GetAll(GetAllPurchaseDebitNotePartyInput input);

        Task<GetPurchaseDebitNotePartyForEditOutput> GetPurchaseDebitNotePartyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseDebitNotePartyDto input);

        Task Delete(EntityDto<long> input);

    }
}