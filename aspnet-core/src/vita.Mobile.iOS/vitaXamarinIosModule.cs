using Abp.Modules;
using Abp.Reflection.Extensions;

namespace vita
{
    [DependsOn(typeof(vitaXamarinSharedModule))]
    public class vitaXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaXamarinIosModule).GetAssembly());
        }
    }
}