using System.Threading.Tasks;
using Abp.Application.Services;
using vita.MultiTenancy.Payments.Dto;
using vita.MultiTenancy.Payments.Stripe.Dto;

namespace vita.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}