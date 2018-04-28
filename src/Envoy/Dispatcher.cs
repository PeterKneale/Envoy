using Envoy.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy
{
    public class Dispatcher : IDispatchCommands, IDispatchRequests, IDispatchEvents
    {
        private readonly IResolver _resolver;
        private readonly IExecuteCommands _commands;
        private readonly IExecuteEvents _events;
        private readonly IExecuteRequests _requests;
        private readonly ILogger _logger;

        public Dispatcher(IResolver resolver, IExecuteCommands commands, IExecuteEvents events, IExecuteRequests requests, ILogger logger = null)
        {
            Guard.AgainstNull(resolver, nameof(resolver));
            Guard.AgainstNull(commands, nameof(commands));
            Guard.AgainstNull(events, nameof(events));
            Guard.AgainstNull(requests, nameof(requests));

            _resolver = resolver;
            _commands = commands;
            _events = events;
            _requests = requests;
            _logger = logger ?? new NullLogger();
        }

        public async Task CommandAsync<T>(T command, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ICommand
        {
            Guard.AgainstNull(command, nameof(command));
            var handler = Resolve<IHandleCommand<T>>();
            await _commands.ExecuteAsync(handler, command, cancellationToken);
        }

        public async Task PublishAsync<T>(T evnt, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent
        {
            Guard.AgainstNull(evnt, nameof(evnt));
            var handler = ResolveAll<IHandleEvent<T>>();
            await _events.ExecuteAsync(handler, evnt, cancellationToken);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default(CancellationToken)) where TRequest : class, IRequest<TResponse>
        {
            Guard.AgainstNull(request, nameof(request));
            var handler = Resolve<IHandleRequest<TRequest, TResponse>>();
            var response = await _requests.ExecuteAsync(handler, request, cancellationToken);
            return response;
        }

        private T Resolve<T>() where T : class, IHandler
        {
            T handler;
            try
            {
                _logger.LogDebug(Text.FindingHandler<T>());
                handler = _resolver.Resolve<T>();
                _logger.LogInfo(Text.FoundHandler<T>(handler));
            }
            catch (Exception ex)
            {
                var msg = Text.HandlerCreationError<T>();
                _logger.LogException(msg, ex);
                throw new Exception(msg, ex);
            }
            if (handler == null)
            {
                var msg = Text.FoundNoHandler<T>();
                _logger.LogError(msg);
                throw new Exception(msg);
            }

            return handler;
        }

        private IEnumerable<T> ResolveAll<T>() where T : class, IHandler
        {
            IEnumerable<T> handlers;
            try
            {
                _logger.LogDebug(Text.FindingHandler<T>());
                handlers = _resolver.ResolveAll<T>();
                foreach (var handler in handlers)
                {
                    _logger.LogInfo(Text.FoundHandler<T>(handler));
                }
            }
            catch (Exception ex)
            {
                var msg = Text.HandlerCreationError<T>();
                _logger.LogError(msg);
                throw new Exception(msg, ex);
            }
            if (handlers.Count() == 0)
            {
                _logger.LogWarn(Text.FoundNoHandler<T>());
            }

            return handlers;
        }
    }
}
