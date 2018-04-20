using System.Threading;
using System.Threading.Tasks;

namespace Envoy.Extras.Autofac.Tests
{
    internal class TestCommand : ICommand
    {
    }

    internal class TestRequest : IRequest<TestResponse>
    {
    }

    internal class TestResponse
    {

    }

    internal class TestEvent : IEvent
    {

    }

    internal class TestCommandHandler : IHandleCommand<TestCommand>
    {
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    internal class TestEventHandler : IHandleEvent<TestEvent>
    {
        public Task HandleAsync(TestEvent evnt, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    internal class TestRequestHandler : IHandleRequest<TestRequest, TestResponse>
    {
        public Task<TestResponse> HandleAsync(IRequest<TestResponse> request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestResponse());
        }
    }
}
