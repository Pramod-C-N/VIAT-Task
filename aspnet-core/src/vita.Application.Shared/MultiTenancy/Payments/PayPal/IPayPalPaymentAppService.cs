using System.Threading.Tasks;
using Abp.Application.Services;
using vita.MultiTenancy.Payments.PayPal.Dto;

namespace vita.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
