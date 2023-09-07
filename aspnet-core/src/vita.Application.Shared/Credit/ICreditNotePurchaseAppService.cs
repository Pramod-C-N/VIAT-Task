using System;
using System.Data;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Credit.Dtos;
using vita.Dto;
using vita.Sales.Dtos;

namespace vita.Credit
{
    public interface ICreditNotePurchaseAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteForViewDto>> GetAll(GetAllCreditNoteInput input);

        //Task<GetCreditNoteForEditOutput> GetCreditNoteForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCreditNoteDto input);

        //Task Delete(EntityDto<long> input);
        Task<InvoiceResponse> CreateCreditNote(CreateOrEditCreditNoteDto input);
        Task<DataTable> GetCreditData(DateTime fromDate, DateTime toDate, DateTime? creationDate, string customername, string salesorderno, string purchaseorderno, string invoicerefno, string buyercode, string shippedcode);

        Task<bool> InsertBatchUploadCreditPurchase(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate);

    }
}