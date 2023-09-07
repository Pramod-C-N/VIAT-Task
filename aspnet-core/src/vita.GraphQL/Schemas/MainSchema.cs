using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using vita.Queries.Container;
using System;

namespace vita.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}