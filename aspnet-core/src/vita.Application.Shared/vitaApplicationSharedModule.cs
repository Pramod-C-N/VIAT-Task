using Abp.Modules;
using Abp.Reflection.Extensions;

namespace vita
{
    [DependsOn(typeof(vitaCoreSharedModule))]
    public class vitaApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaApplicationSharedModule).GetAssembly());
        }
    }
}