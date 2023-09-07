using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;

namespace vita.Debit
{
    public interface IDebitNoteContactPersonsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNoteContactPersonForViewDto>> GetAll(GetAllDebitNoteContactPersonsInput input);

        Task<GetDebitNoteContactPersonForEditOutput> GetDebitNoteContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNoteContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}