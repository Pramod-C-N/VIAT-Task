using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Purchase.Dtos;
using vita.Dto;
using vita.Sales.Dtos;
using System.Data;

namespace vita.Purchase
{
    public interface IPurchaseEntriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetPurchaseEntryForViewDto>> GetAll(GetAllPurchaseEntriesInput input);

        Task<GetPurchaseEntryForEditOutput> GetPurchaseEntryForEdit(EntityDto<long> input);

        Task<long> CreateOrEdit(CreateOrEditPurchaseEntryDto input);

        Task Delete(EntityDto<long> input);
        Task<InvoiceResponse> CreatePurchaseEntry(CreateOrEditPurchaseEntryDto input);
        Task<DataTable> GetPurchaseData(DateTime fromDate, DateTime toDate);


        Task<bool> InsertBatchUploadPurchase(string json, string fileName, int? tenantId, DateTime? fromDate, DateTime? toDate);

    }
}