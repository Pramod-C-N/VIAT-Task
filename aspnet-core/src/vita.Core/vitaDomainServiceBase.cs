using Abp.Domain.Services;

namespace vita
{
    public abstract class vitaDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected vitaDomainServiceBase()
        {
            LocalizationSourceName = vitaConsts.LocalizationSourceName;
        }
    }
}
