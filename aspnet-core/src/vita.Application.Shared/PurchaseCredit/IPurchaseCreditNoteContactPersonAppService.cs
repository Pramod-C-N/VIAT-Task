using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNoteContactPersonAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNoteContactPersonForViewDto>> GetAll(GetAllPurchaseCreditNoteContactPersonInput input);

        Task<GetPurchaseCreditNoteContactPersonForViewDto> GetPurchaseCreditNoteContactPersonForView(long id);

        Task<GetPurchaseCreditNoteContactPersonForEditOutput> GetPurchaseCreditNoteContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseCreditNoteContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}