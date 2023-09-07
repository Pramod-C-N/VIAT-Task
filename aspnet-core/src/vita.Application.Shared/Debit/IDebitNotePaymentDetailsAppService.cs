using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;

namespace vita.Debit
{
    public interface IDebitNotePaymentDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNotePaymentDetailForViewDto>> GetAll(GetAllDebitNotePaymentDetailsInput input);

        Task<GetDebitNotePaymentDetailForEditOutput> GetDebitNotePaymentDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNotePaymentDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}