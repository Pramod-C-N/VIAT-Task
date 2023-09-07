using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;

namespace vita.Debit
{
    public interface IDebitNoteDiscountsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNoteDiscountForViewDto>> GetAll(GetAllDebitNoteDiscountsInput input);

        Task<GetDebitNoteDiscountForEditOutput> GetDebitNoteDiscountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNoteDiscountDto input);

        Task Delete(EntityDto<long> input);

    }
}