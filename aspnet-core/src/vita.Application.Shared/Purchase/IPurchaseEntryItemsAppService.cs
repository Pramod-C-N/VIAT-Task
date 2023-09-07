using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;

namespace vita.Purchase
{
    public interface IPurchaseEntryItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntryItemForViewDto>> GetAll(GetAllPurchaseEntryItemsInput input);

        Task<GetPurchaseEntryItemForEditOutput> GetPurchaseEntryItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseEntryItemDto input);

        Task Delete(EntityDto<long> input);

    }
}