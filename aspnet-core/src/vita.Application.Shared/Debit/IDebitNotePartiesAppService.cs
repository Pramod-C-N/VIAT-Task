using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;

namespace vita.Debit
{
    public interface IDebitNotePartiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNotePartyForViewDto>> GetAll(GetAllDebitNotePartiesInput input);

        Task<GetDebitNotePartyForEditOutput> GetDebitNotePartyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNotePartyDto input);

        Task Delete(EntityDto<long> input);

    }
}