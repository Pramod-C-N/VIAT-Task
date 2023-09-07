using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;

namespace vita.DraftFee
{
    public interface IDraftItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDraftItemForViewDto>> GetAll(GetAllDraftItemsInput input);

        Task<GetDraftItemForViewDto> GetDraftItemForView(long id);

        Task<GetDraftItemForEditOutput> GetDraftItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDraftItemDto input);

        Task Delete(EntityDto<long> input);

    }
}