using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;

namespace vita.Sales
{
    public interface ISalesInvoiceSummariesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoiceSummaryForViewDto>> GetAll(GetAllSalesInvoiceSummariesInput input);

        Task<GetSalesInvoiceSummaryForViewDto> GetSalesInvoiceSummaryForView(long id);

        Task<GetSalesInvoiceSummaryForEditOutput> GetSalesInvoiceSummaryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoiceSummaryDto input);

        Task Delete(EntityDto<long> input);

    }
}