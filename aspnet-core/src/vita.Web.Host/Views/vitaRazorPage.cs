using Abp.AspNetCore.Mvc.Views;

namespace vita.Web.Views
{
    public abstract class vitaRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected vitaRazorPage()
        {
            LocalizationSourceName = vitaConsts.LocalizationSourceName;
        }
    }
}
