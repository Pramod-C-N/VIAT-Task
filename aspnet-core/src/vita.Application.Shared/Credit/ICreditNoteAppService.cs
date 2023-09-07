using System;
using System.Data;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Credit.Dtos;
using vita.Dto;
using vita.Sales.Dtos;

namespace vita.Credit
{
    public interface ICreditNoteAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteForViewDto>> GetAll(GetAllCreditNoteInput input);

        Task<GetCreditNoteForEditOutput> GetCreditNoteForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditCreditNoteDto input);

        Task Delete(EntityDto<Guid> input);
        Task<InvoiceResponse> CreateCreditNote(CreateOrEditCreditNoteDto input);
        Task<DataTable> GetCreditData(DateTime fromDate, DateTime toDate);

        Task<bool> InsertBatchUploadCredit(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate);
        Task<bool> GenerateInvoice_SG(CreateOrEditCreditNoteDto input, int batchId);

    }
}