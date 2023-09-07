using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;

namespace vita.Sales
{
    public interface ISalesInvoiceDiscountsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoiceDiscountForViewDto>> GetAll(GetAllSalesInvoiceDiscountsInput input);

        Task<GetSalesInvoiceDiscountForViewDto> GetSalesInvoiceDiscountForView(long id);

        Task<GetSalesInvoiceDiscountForEditOutput> GetSalesInvoiceDiscountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoiceDiscountDto input);

        Task Delete(EntityDto<long> input);

    }
}