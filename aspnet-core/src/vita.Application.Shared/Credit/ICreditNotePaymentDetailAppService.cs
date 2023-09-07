using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;

namespace vita.Credit
{
    public interface ICreditNotePaymentDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNotePaymentDetailForViewDto>> GetAll(GetAllCreditNotePaymentDetailInput input);

        Task<GetCreditNotePaymentDetailForViewDto> GetCreditNotePaymentDetailForView(long id);

        Task<GetCreditNotePaymentDetailForEditOutput> GetCreditNotePaymentDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNotePaymentDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}