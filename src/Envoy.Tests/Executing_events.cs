using NSubstitute;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Envoy.Tests
{
    public class Executing_events
    {
        [Fact]
        public async Task Multiple_event_handlers_executed()
        {
            // Arrange
            var handlers = new List<IHandleEvent<TestEvent>>
            {
                Substitute.For<IHandleEvent<TestEvent>>(),
                Substitute.For<IHandleEvent<TestEvent>>(),
                Substitute.For<IHandleEvent<TestEvent>>()
            };

            // Act
            var evnt = new TestEvent();
            var executor = new Executor();
            await executor.ExecuteAsync(handlers, evnt);

            // Assert
            await handlers[0].Received(1).HandleAsync(evnt, CancellationToken.None);
            await handlers[1].Received(1).HandleAsync(evnt, CancellationToken.None);
            await handlers[2].Received(1).HandleAsync(evnt, CancellationToken.None);
        }
    }
}
