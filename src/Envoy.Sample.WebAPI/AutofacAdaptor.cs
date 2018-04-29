using Autofac;
using System.Collections.Generic;

namespace Envoy.Sample.WebAPI
{
    public class AutofacAdaptor : IResolver
    {
        private readonly IComponentContext _context;

        public AutofacAdaptor(IComponentContext context)
        {
            _context = context;
        }

        public T Resolve<T>() => _context.Resolve<T>();

        public IEnumerable<T> ResolveAll<T>() => _context.Resolve<IEnumerable<T>>();
    }
}
