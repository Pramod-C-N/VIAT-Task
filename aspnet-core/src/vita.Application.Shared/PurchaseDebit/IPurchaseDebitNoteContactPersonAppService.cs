using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNoteContactPersonAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNoteContactPersonForViewDto>> GetAll(GetAllPurchaseDebitNoteContactPersonInput input);

        Task<GetPurchaseDebitNoteContactPersonForEditOutput> GetPurchaseDebitNoteContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseDebitNoteContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}