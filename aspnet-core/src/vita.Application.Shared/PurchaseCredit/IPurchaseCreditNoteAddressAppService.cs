using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNoteAddressAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNoteAddressForViewDto>> GetAll(GetAllPurchaseCreditNoteAddressInput input);

        Task<GetPurchaseCreditNoteAddressForViewDto> GetPurchaseCreditNoteAddressForView(long id);

        Task<GetPurchaseCreditNoteAddressForEditOutput> GetPurchaseCreditNoteAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseCreditNoteAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}