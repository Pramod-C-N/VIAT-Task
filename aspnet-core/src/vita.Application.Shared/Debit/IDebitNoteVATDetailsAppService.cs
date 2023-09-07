using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;

namespace vita.Debit
{
    public interface IDebitNoteVATDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNoteVATDetailForViewDto>> GetAll(GetAllDebitNoteVATDetailsInput input);

        Task<GetDebitNoteVATDetailForEditOutput> GetDebitNoteVATDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNoteVATDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}