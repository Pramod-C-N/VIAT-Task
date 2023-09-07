using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;

namespace vita.Purchase
{
    public interface IPurchaseEntryVATDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntryVATDetailForViewDto>> GetAll(GetAllPurchaseEntryVATDetailsInput input);

        Task<GetPurchaseEntryVATDetailForEditOutput> GetPurchaseEntryVATDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseEntryVATDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}