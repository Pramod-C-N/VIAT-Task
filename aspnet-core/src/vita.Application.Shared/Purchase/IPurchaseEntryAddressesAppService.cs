using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;

namespace vita.Purchase
{
    public interface IPurchaseEntryAddressesAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntryAddressForViewDto>> GetAll(GetAllPurchaseEntryAddressesInput input);

        Task<GetPurchaseEntryAddressForEditOutput> GetPurchaseEntryAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseEntryAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}