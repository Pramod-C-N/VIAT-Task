using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;

namespace vita.Sales
{
    public interface ISalesInvoiceVATDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoiceVATDetailForViewDto>> GetAll(GetAllSalesInvoiceVATDetailsInput input);

        Task<GetSalesInvoiceVATDetailForEditOutput> GetSalesInvoiceVATDetailForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoiceVATDetailDto input);

        Task Delete(EntityDto<long> input);

    }
}