using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;

namespace vita.Debit
{
    public interface IDebitNoteItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNoteItemForViewDto>> GetAll(GetAllDebitNoteItemsInput input);

        Task<GetDebitNoteItemForEditOutput> GetDebitNoteItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNoteItemDto input);

        Task Delete(EntityDto<long> input);

    }
}