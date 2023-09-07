using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseCredit.Dtos;
using vita.Dto;

namespace vita.PurchaseCredit
{
    public interface IPurchaseCreditNoteSummaryAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseCreditNoteSummaryForViewDto>> GetAll(GetAllPurchaseCreditNoteSummaryInput input);

        Task<GetPurchaseCreditNoteSummaryForViewDto> GetPurchaseCreditNoteSummaryForView(long id);

        Task<GetPurchaseCreditNoteSummaryForEditOutput> GetPurchaseCreditNoteSummaryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseCreditNoteSummaryDto input);

        Task Delete(EntityDto<long> input);

    }
}