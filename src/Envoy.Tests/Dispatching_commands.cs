using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Envoy.Tests
{
    public class Dispatching_commands
    {
        private IResolver _resolver;
        private IExecuteCommands _executor;
        private IHandleCommand<TestCommand> _handler;

        public Dispatching_commands()
        {
            _resolver = Substitute.For<IResolver>();
            _executor = Substitute.For<IExecuteCommands>();
            _handler = Substitute.For<IHandleCommand<TestCommand>>();
        }

        private Dispatcher CreateDispatcher()
        {
            return new Dispatcher(
                _resolver,
                _executor,
                Substitute.For<IExecuteEvents>(),
                Substitute.For<IExecuteRequests>(),
                Substitute.For<ILogger>());
        }

        [Fact]
        public async Task Continues_to_execution_if_handler_resolved()
        {
            // Arrange
            _resolver.Resolve<IHandleCommand<TestCommand>>().Returns(_handler);

            // Act
            var dispatcher = CreateDispatcher();
            var command = new TestCommand();
            await dispatcher.CommandAsync(command);

            // Assert
            _resolver.Received(1).Resolve<IHandleCommand<TestCommand>>();
            await _executor.Received(1).ExecuteAsync(_handler, command, CancellationToken.None);
        }

        [Fact]
        public void Throws_if_handler_not_resolved()
        {
            // Arrange

            // Act
            var dispatcher = CreateDispatcher();
            var result = Assert.ThrowsAsync<Exception>(async () => await dispatcher.CommandAsync(new TestCommand()));

            // Assert
            _resolver.Received(1).Resolve<IHandleCommand<TestCommand>>();
        }

        [Fact]
        public void Throws_if_resolver_could_not_instantiate()
        {
            // Arrange
            _resolver.Resolve<IHandleCommand<TestCommand>>().Returns(x => throw new Exception("cannot instantiate"));

            // Act
            var dispatcher = CreateDispatcher();
            var evnt = new TestCommand();
            var result = Assert.ThrowsAsync<Exception>(async () => await dispatcher.CommandAsync(evnt));

            // Assert
            _resolver.Received(1).Resolve<IHandleCommand<TestCommand>>();
        }
    }
}
