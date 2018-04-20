using Autofac;

namespace Envoy.Extras.Autofac
{
    public class EnvoyAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EnvoyAutofacResolver>().As<IResolver>().SingleInstance();
            builder.RegisterType<Dispatcher>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<Executor>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
