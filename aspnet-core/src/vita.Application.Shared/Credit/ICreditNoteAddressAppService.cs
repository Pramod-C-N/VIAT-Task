using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;

namespace vita.Credit
{
    public interface ICreditNoteAddressAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteAddressForViewDto>> GetAll(GetAllCreditNoteAddressInput input);

        Task<GetCreditNoteAddressForViewDto> GetCreditNoteAddressForView(long id);

        Task<GetCreditNoteAddressForEditOutput> GetCreditNoteAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNoteAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}