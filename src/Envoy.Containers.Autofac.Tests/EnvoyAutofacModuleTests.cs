using Autofac;
using Xunit;

namespace Envoy.Containers.Autofac.Tests
{
    public class EnvoyAutofacModuleTests
    {
        private IContainer _container;

        public EnvoyAutofacModuleTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EnvoyAutofacModule>();
            _container = builder.Build();
        }

        [Fact]
        public void Command_dispatcher_can_be_resolved()
        {
            Assert.NotNull(_container.Resolve<IDispatchCommand>());
        }

        [Fact]
        public void Request_dispatcher_can_be_resolved()
        {
            Assert.NotNull(_container.Resolve<IDispatchRequest>());
        }

        [Fact]
        public void Event_dispatcher_can_be_resolved()
        {
            Assert.NotNull(_container.Resolve<IDispatchEvent>());
        }
        [Fact]
        public void Resolver_can_be_resolved()
        {
            Assert.NotNull(_container.Resolve<IResolver>());
        }
    }
}


