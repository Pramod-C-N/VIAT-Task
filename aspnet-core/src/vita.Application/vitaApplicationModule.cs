using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using vita.Authorization;

namespace vita
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(vitaApplicationSharedModule),
        typeof(vitaCoreModule)
        )]
    public class vitaApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaApplicationModule).GetAssembly());
        }
    }
}