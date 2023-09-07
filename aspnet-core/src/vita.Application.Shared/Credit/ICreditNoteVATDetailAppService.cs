using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;

namespace vita.Credit
{
    public interface ICreditNoteVATDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteVATDetailForViewDto>> GetAll(GetAllCreditNoteVATDetailInput input);

        Task<GetCreditNoteVATDetailForEditOutput> GetCreditNoteVATDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNoteVATDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}