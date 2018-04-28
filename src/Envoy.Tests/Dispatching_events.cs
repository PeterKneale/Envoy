using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Envoy.Tests
{
    public class Dispatching_events
    {
        private IResolver _resolver;
        private ILogger _logger;
        private IExecuteEvents _executor;
        private IHandleEvent<TestEvent> _handler;

        public Dispatching_events()
        {
            _resolver = Substitute.For<IResolver>();
            _executor = Substitute.For<IExecuteEvents>();
            _handler = Substitute.For<IHandleEvent<TestEvent>>();
            _logger = Substitute.For<ILogger>();
        }

        [Fact]
        public async Task Continues_to_execution_if_handler_resolved()
        {
            // Arrange
            var handlers = new List<IHandleEvent<TestEvent>> { _handler };
            _resolver.ResolveAll<IHandleEvent<TestEvent>>().Returns(handlers);

            // Act
            var dispatcher = CreateDispatcher();
            var evnt = new TestEvent();
            await dispatcher.PublishAsync(evnt);

            // Assert
            _resolver.Received(1).ResolveAll<IHandleEvent<TestEvent>>();
            await _executor.Received(1).ExecuteAsync(handlers, evnt, CancellationToken.None);
        }

        [Fact]
        public async Task Warns_if_handler_not_resolved()
        {
            // Arrange

            // Act
            var dispatcher = CreateDispatcher();
            var evnt = new TestEvent();
            await dispatcher.PublishAsync(evnt);

            // Assert
            _resolver.Received(1).ResolveAll<IHandleEvent<TestEvent>>();
            _logger.Received(1).LogWarn(Arg.Is<string>(x => x.Contains("No handler could be resolved")));
        }

        [Fact]
        public void Throws_if_resolver_could_not_instantiate()
        {
            // Arrange
            _resolver.ResolveAll<IHandleEvent<TestEvent>>().Returns(x => throw new Exception("cannot instantiate"));
            // Act
            var dispatcher = CreateDispatcher();
            var evnt = new TestEvent();
            var result = Assert.ThrowsAsync<Exception>(async () => await dispatcher.PublishAsync(evnt));

            // Assert
            _resolver.Received(1).ResolveAll<IHandleEvent<TestEvent>>();
        }

        private Dispatcher CreateDispatcher()
        {
            return new Dispatcher(
                _resolver,
                Substitute.For<IExecuteCommands>(),
                _executor,
                Substitute.For<IExecuteRequests>(),
                _logger);
        }
    }
}
