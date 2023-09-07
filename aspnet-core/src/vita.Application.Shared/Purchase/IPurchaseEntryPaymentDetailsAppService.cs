using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;

namespace vita.Purchase
{
    public interface IPurchaseEntryPaymentDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntryPaymentDetailForViewDto>> GetAll(GetAllPurchaseEntryPaymentDetailsInput input);

        Task<GetPurchaseEntryPaymentDetailForEditOutput> GetPurchaseEntryPaymentDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseEntryPaymentDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}