using System.Threading.Tasks;
using vita.Authorization.Users;

namespace vita.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
