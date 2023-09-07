using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.Sales.Dtos;
using vita.Dto;

namespace vita.Sales
{
    public interface ISalesInvoicePartiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesInvoicePartyForViewDto>> GetAll(GetAllSalesInvoicePartiesInput input);

        Task<GetSalesInvoicePartyForViewDto> GetSalesInvoicePartyForView(long id);

        Task<GetSalesInvoicePartyForEditOutput> GetSalesInvoicePartyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSalesInvoicePartyDto input);

        Task Delete(EntityDto<long> input);

    }
}