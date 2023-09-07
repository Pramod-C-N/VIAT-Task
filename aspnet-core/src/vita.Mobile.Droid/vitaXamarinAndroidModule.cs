using Abp.Modules;
using Abp.Reflection.Extensions;

namespace vita
{
    [DependsOn(typeof(vitaXamarinSharedModule))]
    public class vitaXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaXamarinAndroidModule).GetAssembly());
        }
    }
}