using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;

namespace vita.Purchase
{
    public interface IPurchaseEntryContactPersonsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntryContactPersonForViewDto>> GetAll(GetAllPurchaseEntryContactPersonsInput input);

        Task<GetPurchaseEntryContactPersonForEditOutput> GetPurchaseEntryContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseEntryContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}