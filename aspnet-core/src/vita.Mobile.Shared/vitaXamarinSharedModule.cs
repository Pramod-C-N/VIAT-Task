using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace vita
{
    [DependsOn(typeof(vitaClientModule), typeof(AbpAutoMapperModule))]
    public class vitaXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaXamarinSharedModule).GetAssembly());
        }
    }
}