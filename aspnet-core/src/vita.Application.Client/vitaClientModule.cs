using Abp.Modules;
using Abp.Reflection.Extensions;

namespace vita
{
    public class vitaClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaClientModule).GetAssembly());
        }
    }
}
