using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy
{
    public class Dispatcher : IDispatchCommand, IDispatchRequest, IDispatchEvent
    {
        private readonly IResolver _resolver;
        private readonly IExecuteCommands _commands;
        private readonly IExecuteEvents _events;
        private readonly IExecuteRequests _requests;

        public Dispatcher(IResolver resolver, IExecuteCommands commands, IExecuteEvents events, IExecuteRequests requests)
        {
            _resolver = resolver;
            _commands = commands;
            _events = events;
            _requests = requests;
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
            var response = await _requests.Execute(handler, request, cancellationToken);
            return response;
        }

        private T Resolve<T>() where T : class, IHandler
        {
            T handler;
            try
            {
                Trace.WriteLine(Text.FindingHandler<T>());
                handler = _resolver.Resolve<T>();
                Trace.WriteLine(Text.FoundHandler<T>(handler));
            }
            catch (Exception ex)
            {
                throw new EnvoyException(Text.HandlerCreationError<T>(), ex);
            }
            if (handler == null)
            {
                throw new EnvoyException(Text.FoundNoHandler<T>());
            }

            return handler;
        }

        private IEnumerable<T> ResolveAll<T>() where T : class, IHandler
        {
            IEnumerable<T> handlers;
            try
            {
                Trace.WriteLine(Text.FindingHandler<T>());
                handlers = _resolver.ResolveAll<T>();
                foreach (var handler in handlers)
                {
                    Trace.WriteLine(Text.FoundHandler<T>(handler));
                }
            }
            catch (Exception ex)
            {
                throw new EnvoyException(Text.HandlerCreationError<T>(), ex);
            }
            if (handlers.Count() == 0)
            {
                Trace.WriteLine(Text.FoundNoHandler<T>());
            }

            return handlers;
        }
    }
}
