using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;

namespace vita.Purchase
{
    public interface IPurchaseEntryDiscountsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntryDiscountForViewDto>> GetAll(GetAllPurchaseEntryDiscountsInput input);

        Task<GetPurchaseEntryDiscountForEditOutput> GetPurchaseEntryDiscountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseEntryDiscountDto input);

        Task Delete(EntityDto<long> input);

    }
}