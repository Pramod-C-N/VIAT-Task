using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;

namespace vita.Credit
{
    public interface ICreditNoteSummaryAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteSummaryForViewDto>> GetAll(GetAllCreditNoteSummaryInput input);

        Task<GetCreditNoteSummaryForEditOutput> GetCreditNoteSummaryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNoteSummaryDto input);

        Task Delete(EntityDto<long> input);

    }
}