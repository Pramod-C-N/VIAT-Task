using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;

namespace vita.Debit
{
    public interface IDebitNoteSummariesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNoteSummaryForViewDto>> GetAll(GetAllDebitNoteSummariesInput input);

        Task<GetDebitNoteSummaryForEditOutput> GetDebitNoteSummaryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNoteSummaryDto input);

        Task Delete(EntityDto<long> input);

    }
}