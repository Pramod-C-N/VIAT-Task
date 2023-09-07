using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;

namespace vita.Purchase
{
    public interface IPurchaseEntryPartiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntryPartyForViewDto>> GetAll(GetAllPurchaseEntryPartiesInput input);

        Task<GetPurchaseEntryPartyForEditOutput> GetPurchaseEntryPartyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseEntryPartyDto input);

        Task Delete(EntityDto<long> input);

    }
}