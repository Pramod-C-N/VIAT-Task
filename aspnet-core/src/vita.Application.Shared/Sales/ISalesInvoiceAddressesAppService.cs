using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;

namespace vita.Sales
{
    public interface ISalesInvoiceAddressesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoiceAddressForViewDto>> GetAll(GetAllSalesInvoiceAddressesInput input);

        Task<GetSalesInvoiceAddressForViewDto> GetSalesInvoiceAddressForView(long id);

        Task<GetSalesInvoiceAddressForEditOutput> GetSalesInvoiceAddressForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoiceAddressDto input);

        Task Delete(EntityDto<long> input);

    }
}