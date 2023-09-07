using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;

namespace vita.Credit
{
    public interface ICreditNoteDiscountAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteDiscountForViewDto>> GetAll(GetAllCreditNoteDiscountInput input);

        Task<GetCreditNoteDiscountForEditOutput> GetCreditNoteDiscountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNoteDiscountDto input);

        Task Delete(EntityDto<long> input);

    }
}