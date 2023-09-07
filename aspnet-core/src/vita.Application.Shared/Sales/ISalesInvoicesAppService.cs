using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;
using System.Data;

namespace vita.Sales
{
    public interface ISalesInvoicesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoiceForViewDto>> GetAll(GetAllSalesInvoicesInput input);

        Task<GetSalesInvoiceForViewDto> GetSalesInvoiceForView(long id);

        Task<GetSalesInvoiceForEditOutput> GetSalesInvoiceForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoiceDto input);

        Task Delete(EntityDto<long> input);
        Task<InvoiceResponse> CreateSalesInvoice(CreateOrEditSalesInvoiceDto input);
        Task<bool> InsertBatchUploadSales(string json,string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate); 
        
        Task<DataTable> GetSalesData(DateTime fromDate, DateTime toDate);

        Task<DataTable> GetSalesBatchData(string filename);

        Task<DataTable> GetSalesInvalidRecord(int batchid);

        Task<int> GetLatestBatchId();
        Task<bool> GenerateInvoice_SG(CreateOrEditSalesInvoiceDto input, int batchId);


    }
}