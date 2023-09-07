using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;

namespace vita.Credit
{
    public interface ICreditNoteItemAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteItemForViewDto>> GetAll(GetAllCreditNoteItemInput input);

        Task<GetCreditNoteItemForViewDto> GetCreditNoteItemForView(long id);

        Task<GetCreditNoteItemForEditOutput> GetCreditNoteItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNoteItemDto input);

        Task Delete(EntityDto<long> input);

    }
}