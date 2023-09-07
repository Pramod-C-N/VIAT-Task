using System;
using System.Data;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Debit.Dtos;
using vita.Dto;
using vita.Sales.Dtos;

namespace vita.Debit
{
    public interface IDebitNotesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNoteForViewDto>> GetAll(GetAllDebitNotesInput input);

        Task<GetDebitNoteForEditOutput> GetDebitNoteForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDebitNoteDto input);

        Task Delete(EntityDto<long> input);
        Task<InvoiceResponse> CreateDebitNote(CreateOrEditDebitNoteDto input);
        Task<DataTable> GetDebitData(DateTime fromDate, DateTime toDate, DateTime? creationDate, string customername, string salesorderno, string purchaseorderno, string invoicerefno, string buyercode, string shippedcode, string IRNo,string createdby);

        Task<bool> InsertBatchUploadDebitSales(string json, string fileName, int? tenantId, DateTime? fromdate, DateTime? todate);

        Task<bool> InsertBatchUploadDebitPurchase(string json, string fileName, int? tenantId, DateTime? fromdate, DateTime? todate);

        Task<bool> InsertDebitReportData(long IRNNo);
        //Task<bool> GenerateInvoice_SG(CreateOrEditDebitNoteDto input, int batchId);


    }
}