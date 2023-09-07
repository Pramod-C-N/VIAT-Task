using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNoteVATDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNoteVATDetailForViewDto>> GetAll(GetAllPurchaseDebitNoteVATDetailInput input);

        Task<GetPurchaseDebitNoteVATDetailForEditOutput> GetPurchaseDebitNoteVATDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseDebitNoteVATDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}