using System.Threading;
using System.Threading.Tasks;

namespace Envoy.Tests
{
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
        public Task Handle(TestCommand command, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class TestEventHandler : IHandleEvent<TestEvent>
    {
        public Task HandleAsync(TestEvent evnt, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class TestRequestHandler : IHandleRequest<TestRequest, TestResponse>
    {
        public Task<TestResponse> HandleAsync(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestResponse());
        }
    }
}
