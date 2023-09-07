using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;

namespace vita.Sales
{
    public interface ISalesInvoicePaymentDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoicePaymentDetailForViewDto>> GetAll(GetAllSalesInvoicePaymentDetailsInput input);

        Task<GetSalesInvoicePaymentDetailForViewDto> GetSalesInvoicePaymentDetailForView(long id);

        Task<GetSalesInvoicePaymentDetailForEditOutput> GetSalesInvoicePaymentDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoicePaymentDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}