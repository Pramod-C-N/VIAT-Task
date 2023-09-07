using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;

namespace vita.Credit
{
    public interface ICreditNoteContactPersonAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteContactPersonForViewDto>> GetAll(GetAllCreditNoteContactPersonInput input);

        Task<GetCreditNoteContactPersonForEditOutput> GetCreditNoteContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNoteContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}