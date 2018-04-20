using System.Collections.Generic;
using Autofac;

namespace Envoy.Containers.Autofac
{
    public class EnvoyAutofacResolver : IResolver
    {
        private readonly IComponentContext context;

        public EnvoyAutofacResolver(IComponentContext context)
        {
            this.context = context;
        }

        public T Resolve<T>() => context.Resolve<T>();

        public IEnumerable<T> ResolveAll<T>() => context.Resolve<IEnumerable<T>>();
    }
}
