using Microsoft.Extensions.Configuration;

namespace vita.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
