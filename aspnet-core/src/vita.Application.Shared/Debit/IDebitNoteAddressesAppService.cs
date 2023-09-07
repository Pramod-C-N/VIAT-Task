using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;

namespace vita.Debit
{
    public interface IDebitNoteAddressesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNoteAddressForViewDto>> GetAll(GetAllDebitNoteAddressesInput input);

        Task<GetDebitNoteAddressForEditOutput> GetDebitNoteAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNoteAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}