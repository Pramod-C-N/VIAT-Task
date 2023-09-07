using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;

namespace vita.Credit
{
    public interface ICreditNotePartyAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNotePartyForViewDto>> GetAll(GetAllCreditNotePartyInput input);

        Task<GetCreditNotePartyForEditOutput> GetCreditNotePartyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNotePartyDto input);

        Task Delete(EntityDto<long> input);

    }
}