using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace vita.Web.Public.Views
{
    public abstract class vitaRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected vitaRazorPage()
        {
            LocalizationSourceName = vitaConsts.LocalizationSourceName;
        }
    }
}
