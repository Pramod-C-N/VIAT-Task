using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace vita.Startup
{
    [DependsOn(typeof(vitaCoreModule))]
    public class vitaGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}