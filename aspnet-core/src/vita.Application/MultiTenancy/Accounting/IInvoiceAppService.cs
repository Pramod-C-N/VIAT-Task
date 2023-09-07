using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using vita.MultiTenancy.Accounting.Dto;

namespace vita.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
