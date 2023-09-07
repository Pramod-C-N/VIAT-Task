using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;

namespace vita.DraftFee
{
    public interface IDraftAddressesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDraftAddressForViewDto>> GetAll(GetAllDraftAddressesInput input);

        Task<GetDraftAddressForViewDto> GetDraftAddressForView(long id);

        Task<GetDraftAddressForEditOutput> GetDraftAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDraftAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}