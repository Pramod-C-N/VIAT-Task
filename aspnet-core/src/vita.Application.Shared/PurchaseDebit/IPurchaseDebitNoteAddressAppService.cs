using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNoteAddressAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNoteAddressForViewDto>> GetAll(GetAllPurchaseDebitNoteAddressInput input);

        Task<GetPurchaseDebitNoteAddressForViewDto> GetPurchaseDebitNoteAddressForView(long id);

        Task<GetPurchaseDebitNoteAddressForEditOutput> GetPurchaseDebitNoteAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseDebitNoteAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}