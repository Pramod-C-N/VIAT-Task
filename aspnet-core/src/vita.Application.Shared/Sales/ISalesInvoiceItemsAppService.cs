using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;

namespace vita.Sales
{
    public interface ISalesInvoiceItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoiceItemForViewDto>> GetAll(GetAllSalesInvoiceItemsInput input);

        Task<GetSalesInvoiceItemForViewDto> GetSalesInvoiceItemForView(long id);

        Task<GetSalesInvoiceItemForEditOutput> GetSalesInvoiceItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoiceItemDto input);

        Task Delete(EntityDto<long> input);

    }
}