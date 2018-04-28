using Autofac;

namespace Envoy.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Register your application
            builder.RegisterType<App>()
                .As<IApp>();

            // register your handlers
            builder.RegisterType<TestCommandHandler>().As<IHandleCommand<TestCommand>>();
            builder.RegisterType<TestRequestHandler>().As<IHandleRequest<TestRequest, TestResponse>>();
            builder.RegisterType<TestEvent1Handler>().As<IHandleEvent<TestEvent>>();
            builder.RegisterType<TestEvent2Handler>().As<IHandleEvent<TestEvent>>();

            // Register the required infrastructure
            builder.RegisterType<Dispatcher>()
                .As<IDispatchCommands>()
                .As<IDispatchEvents>()
                .As<IDispatchRequests>();
            builder.RegisterType<Executor>()
                .As<IExecuteCommands>()
                .As<IExecuteEvents>()
                .As<IExecuteRequests>();
            builder.RegisterType<TraceLogger>()
                .As<ILogger>();
            
            // Register a container adaptor of choice
            builder.RegisterType<AutofacAdaptor>()
                .As<IResolver>();
            
            var container = builder.Build();

            // start your application
            container.Resolve<IApp>().Start();
        }
    }
}
