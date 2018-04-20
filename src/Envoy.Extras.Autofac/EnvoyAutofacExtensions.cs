using Autofac;
using Envoy.Extras.Autofac;
using System.Linq;
using System.Reflection;

// Namespace changed to Envoy for easier discovery
// of this extension method.
namespace Envoy
{
    public static class EnvoyAutofacExtensions
    {
        public static void RegisterEnvoy(this ContainerBuilder builder)
        {
            builder.RegisterModule<EnvoyAutofacModule>();
        }

        public static void RegisterEnvoyHandlers(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetTypeInfo()
                    .ImplementedInterfaces.Any(i =>
                        i.IsGenericType &&
                            (
                                i.GetGenericTypeDefinition() == typeof(IHandleCommand<>) ||
                                i.GetGenericTypeDefinition() == typeof(IHandleRequest<,>) ||
                                i.GetGenericTypeDefinition() == typeof(IHandleEvent<>)
                            )))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
