using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;

namespace vita.Sales
{
    public interface ISalesInvoiceContactPersonsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoiceContactPersonForViewDto>> GetAll(GetAllSalesInvoiceContactPersonsInput input);

        Task<GetSalesInvoiceContactPersonForViewDto> GetSalesInvoiceContactPersonForView(long id);

        Task<GetSalesInvoiceContactPersonForEditOutput> GetSalesInvoiceContactPersonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoiceContactPersonDto input);

        Task Delete(EntityDto<long> input);

    }
}