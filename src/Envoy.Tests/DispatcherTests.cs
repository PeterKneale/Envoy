using Autofac;
using Envoy.Containers.Autofac;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Envoy.Tests
{
    public class DispatcherTests
    {
        private readonly Dispatcher _dispatcher;
        private readonly IResolver _resolver;
        private readonly IExecuteCommands _commandExecutor;
        private readonly IExecuteEvents _eventExecutor;
        private readonly IExecuteRequests _requestExecutor;
        private readonly IHandleCommand<TestCommand> _commandHandler;
        private readonly IEnumerable<IHandleEvent<TestEvent>> _eventHandlers;
        private readonly IHandleRequest<TestRequest, TestResponse> _requestHandler;
        private readonly TestCommand _command;
        private readonly TestEvent _event;
        private readonly TestRequest _request;
        private readonly TestResponse _response;
        private readonly CancellationTokenSource _tokenSource;
        private readonly CancellationToken _token;

        public DispatcherTests()
        {
            _resolver = Substitute.For<IResolver>();
            _commandExecutor = Substitute.For<IExecuteCommands>();
            _eventExecutor = Substitute.For<IExecuteEvents>();
            _requestExecutor = Substitute.For<IExecuteRequests>();

            _dispatcher = new Dispatcher(_resolver, _commandExecutor, _eventExecutor, _requestExecutor);

            _commandHandler = Substitute.For<IHandleCommand<TestCommand>>();
            _eventHandlers = new IHandleEvent<TestEvent>[] { Substitute.For<IHandleEvent<TestEvent>>() };
            _requestHandler = Substitute.For<IHandleRequest<TestRequest, TestResponse>>();

            _resolver.Resolve<IHandleCommand<TestCommand>>().Returns(_commandHandler);
            _resolver.ResolveAll<IHandleEvent<TestEvent>>().Returns(_eventHandlers);
            _resolver.Resolve<IHandleRequest<TestRequest, TestResponse>>().Returns(_requestHandler);

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;

            _command = new TestCommand();
            _event = new TestEvent();
            _request = new TestRequest();
            _response = new TestResponse();

            _requestExecutor.Execute(_requestHandler, _request, _token).Returns(_response);
        }

        [Fact]
        public async Task Dispatch_command()
        {
            // act
            await _dispatcher.CommandAsync(_command, _token);

            // assert
            Received.InOrder(async () =>
            {
                _resolver.Resolve<IHandleCommand<TestCommand>>();
                await _commandExecutor.ExecuteAsync(_commandHandler, _command, _token);
            });
        }

        [Fact]
        public async Task Dispatch_event()
        {
            // act
            await _dispatcher.PublishAsync(_event, _token);

            // assert
            Received.InOrder(async () =>
            {
                _resolver.ResolveAll<IHandleEvent<TestEvent>>();
                await _eventExecutor.ExecuteAsync(_eventHandlers, _event, _token);
            });
        }

        [Fact]
        public async Task Dispatch_request()
        {
            // act
            var response = await _dispatcher.RequestAsync<TestRequest, TestResponse>(_request, _token);

            // assert
            Received.InOrder(async () =>
            {
                _resolver.Resolve<IHandleRequest<TestRequest, TestResponse>>();
                await _requestExecutor.Execute(_requestHandler, _request, _token);
            });
            Assert.Equal(_response, response);
        }
    }


}
