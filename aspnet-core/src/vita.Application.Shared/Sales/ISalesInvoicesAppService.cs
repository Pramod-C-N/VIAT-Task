using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;
using System.Data;
using System.Collections.Generic;

namespace vita.Sales
{
    public interface ISalesInvoicesAppService : IApplicationService
    {

        Task<bool> CreateOrUpdateFileMappings(FileMappingPost input);
        Task<string> GetFileMappingById(int id);
        Task<DataTable> GetFileMappings();
        Task<PagedResultDto<GetSalesInvoiceForViewDto>> GetAll(GetAllSalesInvoicesInput input);

        Task<GetSalesInvoiceForViewDto> GetSalesInvoiceForView(long id);

        Task<GetSalesInvoiceForEditOutput> GetSalesInvoiceForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoiceDto input);

        Task Delete(EntityDto<long> input);
        Task<InvoiceResponse> CreateSalesInvoice(CreateOrEditSalesInvoiceDto input);
        Task<bool> InsertBatchUploadSales(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate,bool isVita=true);

        Task<bool> InsertUploadDatatoLogs(string text, string date);

        Task<DataTable> GetSalesData(DateTime fromDate, DateTime toDate, DateTime? creationDate, string customername, string salesorderno, string purchaseorderno, string invoicerefno, string buyercode, string shippedcode,string IRNo,string createdby);

        Task<DataTable> ViewInvoice(string irrno,string type);

        Task<DataTable> GetSalesBatchData(string filename);

        Task<DataTable> GetSalesInvalidRecord(int batchid);

        Task<int> GetLatestBatchId();
        Task<InvoiceResponse> GenerateInvoice_SG(CreateOrEditSalesInvoiceDto input, int batchId);

        void XmlPdfJob(CreateOrEditSalesInvoiceDto input, Guid uuid, string invoiceno, int batchId, bool isPhase1);
        Task<InvoiceResponse> GetSalesInvoiceData(int? irnNo, int tenantId, string transType);

        Task<List<GetPdfIrnData>> GetIrnForFileUpload(int batchid);
        Task<bool> UpdateInvoiceURL(InvoiceResponse response,string type);

        Task<bool> CreateInvoiceFromDraft(string IRNNo);

    }
}