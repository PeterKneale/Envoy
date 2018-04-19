using Autofac;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterEnvoy();
            builder.RegisterEnvoyHandlers(typeof(Program).Assembly);
            
            var container = builder.Build();
            container.Resolve<IDispatchCommand>().CommandAsync(new TestCommand());
            container.Resolve<IDispatchEvent>().PublishAsync(new TestEvent());
            container.Resolve<IDispatchRequest>().RequestAsync<TestRequest, TestResponse>(new TestRequest());
        }
    }

    public class TestCommand : ICommand
    {
    }

    public class TestRequest : IRequest<TestResponse>
    {
    }

    public class TestResponse
    {

    }

    public class TestEvent : IEvent
    {

    }

    public class TestCommandHandler : IHandleCommand<TestCommand>
    {
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
        {
            Trace.WriteLine($"{this.GetType().Name} is handling {command.GetType().Name}");
            return Task.CompletedTask;
        }
    }

    public class TestEvent1Handler : IHandleEvent<TestEvent>
    {
        public Task HandleAsync(TestEvent evnt, CancellationToken cancellationToken)
        {
            Trace.WriteLine($"{this.GetType().Name} is handling {evnt.GetType().Name}");
            return Task.CompletedTask;
        }
    }

    public class TestEvent2Handler : IHandleEvent<TestEvent>
    {
        public Task HandleAsync(TestEvent evnt, CancellationToken cancellationToken)
        {
            Trace.WriteLine($"{this.GetType().Name} is handling {evnt.GetType().Name}");
            return Task.CompletedTask;
        }
    }

    public class TestRequestHandler : IHandleRequest<TestRequest, TestResponse>
    {
        public Task<TestResponse> HandleAsync(IRequest<TestResponse> request, CancellationToken cancellationToken)
        {
            Trace.WriteLine($"{this.GetType().Name} is handling {request.GetType().Name}");
            return Task.FromResult(new TestResponse());
        }
    }
}
