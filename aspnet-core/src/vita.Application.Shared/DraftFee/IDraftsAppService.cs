using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.DraftFee.Dtos;
using vita.Dto;
using vita.Sales.Dtos;

namespace vita.DraftFee
{
    public interface IDraftsAppService : IApplicationService
    {
        Task<InvoiceResponse> CreateDraft(CreateOrEditDraftDto input);        
        Task<PagedResultDto<GetDraftForViewDto>> GetAll(GetAllDraftsInput input);

        Task<GetDraftForEditOutput> GetDraftForEdit(EntityDto<long> input);

        Task<(string,Guid)> CreateOrEdit(CreateOrEditDraftDto input);

        Task Delete(EntityDto<long> input);

        Task<CreateOrEditDraftDto> GetDraftInvoice(int? irnNo, int tenantId, string transType);
    }
}