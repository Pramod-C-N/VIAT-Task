using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using vita.Configure;
using vita.Startup;
using vita.Test.Base;

namespace vita.GraphQL.Tests
{
    [DependsOn(
        typeof(vitaGraphQLModule),
        typeof(vitaTestBaseModule))]
    public class vitaGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(vitaGraphQLTestModule).GetAssembly());
        }
    }
}