using System;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy.Sample
{
    public class TestCommandHandler : IHandleCommand<TestCommand>
    {
        public Task Handle(TestCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine("handling the command");
            return Task.FromResult(0);
        }
    }
    
    public class TestRequestHandler : IHandleRequest<TestRequest, TestResponse>
    {
        public Task<TestResponse> HandleAsync(TestRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine("handling the request");
            return Task.FromResult(new TestResponse());
        }
    }

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
}
