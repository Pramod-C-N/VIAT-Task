using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;

namespace vita.Purchase
{
    public interface IPurchaseEntrySummariesAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntrySummaryForViewDto>> GetAll(GetAllPurchaseEntrySummariesInput input);

        Task<GetPurchaseEntrySummaryForEditOutput> GetPurchaseEntrySummaryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseEntrySummaryDto input);

        Task Delete(EntityDto<long> input);

    }
}