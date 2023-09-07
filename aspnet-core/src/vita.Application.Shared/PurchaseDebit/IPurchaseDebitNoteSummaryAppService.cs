using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.PurchaseDebit.Dtos;
using vita.Dto;

namespace vita.PurchaseDebit
{
    public interface IPurchaseDebitNoteSummaryAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseDebitNoteSummaryForViewDto>> GetAll(GetAllPurchaseDebitNoteSummaryInput input);

        Task<GetPurchaseDebitNoteSummaryForEditOutput> GetPurchaseDebitNoteSummaryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPurchaseDebitNoteSummaryDto input);

        Task Delete(EntityDto<long> input);

    }
}