- Sample command, request and event.
```cs
    
    public class TestCommand : ICommand { }

    public class TestRequest : IRequest<TestResponse> { }
    public class TestResponse { }

    public class TestEvent : IEvent { }
```

- Sample command handler
```cs
    public class TestCommandHandler : IHandleCommand<TestCommand>
    {
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
```

- Sample request handler
```cs
    public class TestRequestHandler : IHandleRequest<TestRequest, TestResponse>
    {
        public Task<TestResponse> HandleAsync(IRequest<TestResponse> request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestResponse());
        }
    }
```

- A sample event handlers
```cs
    public class TestEvent1Handler : IHandleEvent<TestEvent>
    {
        public Task HandleAsync(TestEvent evnt, CancellationToken cancellationToken)
        {
            Console.WriteLine("handling the event in the first handler");
            return Task.FromResult(0);
        }
    }

    public class TestEvent2Handler : IHandleEvent<TestEvent>
    {
        public Task HandleAsync(TestEvent evnt, CancellationToken cancellationToken)
        {
            Console.WriteLine("handling the event in the second handler");
            return Task.FromResult(0);
        }
    }
```

- A sample application
```cs
    public interface IApp
    {
        void Start();
    }

    public class App : IApp
    {
        private readonly IDispatchCommands _commands;
        private readonly IDispatchEvents _events;
        private readonly IDispatchRequests _requests;

        public App(IDispatchCommands commands, IDispatchEvents events, IDispatchRequests requests)
        {
            _commands = commands;
            _events = events;
            _requests = requests;
        }

        public void Start()
        {
            _commands.CommandAsync(new TestCommand());
            _events.PublishAsync(new TestEvent());
            _requests.RequestAsync<TestRequest, TestResponse>(new TestRequest());
        }
    }
```

- A sample container adaptor
```cs
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
```

- Wiring it all up
```cs
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
```

