using Abp.AspNetCore.Mvc.ViewComponents;

namespace vita.Web.Public.Views
{
    public abstract class vitaViewComponent : AbpViewComponent
    {
        protected vitaViewComponent()
        {
            LocalizationSourceName = vitaConsts.LocalizationSourceName;
        }
    }
}