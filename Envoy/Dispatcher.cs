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

        private T Resolve<T>()
        {
            T handler;
            try
            {
                Trace.WriteLine($"Attemping to find handler of type {typeof(T).ToGenericName()}");
                handler = _resolver.Resolve<T>();
                Trace.WriteLine($"Found handler {handler.GetType().ToGenericName()} of type {typeof(T).ToGenericName()}");
            }
            catch (Exception ex)
            {
                throw new EnvoyException(GetHandlerErrorMessage<T>(), ex);
            }
            if (handler == null)
            {
                throw new EnvoyException(GetHandlerMissingMessage<T>());
            }

            return handler;
        }

        private IEnumerable<T> ResolveAll<T>()
        {
            IEnumerable<T> handlers;
            try
            {
                Trace.WriteLine($"Attemping to find handler of type {typeof(T).ToGenericName()}");
                handlers = _resolver.ResolveAll<T>();
                foreach (var handler in handlers)
                {
                    Trace.WriteLine($"Found handler {handler.GetType().ToGenericName()} of type {typeof(T).ToGenericName()}");
                }
            }
            catch (Exception ex)
            {
                throw new EnvoyException(GetHandlerErrorMessage<T>(), ex);
            }
            if (handlers.Count() == 0)
            {
                Trace.WriteLine($"No handlers were found for {typeof(T).ToGenericName()}");
            }

            return handlers;
        }

        private static string GetHandlerErrorMessage<T>()
        {
            return $"Handler for {typeof(T).Name} could not be instantiated.";
        }

        private static string GetHandlerMissingMessage<T>()
        {
            return $"Handler for {typeof(T).Name} could not be found.";
        }
    }
}
