using Abp.Modules;
using Abp.Reflection.Extensions;

namespace vita
{
    public class vitaCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaCoreSharedModule).GetAssembly());
        }
    }
}