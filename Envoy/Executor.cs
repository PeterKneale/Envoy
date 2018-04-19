using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy
{
    public class Executor : IExecuteCommands, IExecuteEvents, IExecuteRequests
    {
        private readonly IResolver _resolver;

        public Executor(IResolver resolver)
        {
            _resolver = resolver;
        }

        public async Task ExecuteAsync<T>(IHandleCommand<T> handler, T command, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ICommand
        {
            LogBefore(handler, command);
            await handler.HandleAsync(command, cancellationToken);
            LogAfter(handler, command);
        }

        public async Task ExecuteAsync<T>(IEnumerable<IHandleEvent<T>> handlers, T evnt, CancellationToken cancellationToken) where T : class, IEvent
        {
            foreach(var handler in handlers)
            {
                LogBefore(handler, evnt);
                await handler.HandleAsync(evnt, cancellationToken);
                LogAfter(handler, evnt);
            }
        }
        
        public async Task<TResponse> Execute<TRequest, TResponse>(IHandleRequest<TRequest, TResponse> handler, TRequest request, CancellationToken cancellationToken) where TRequest:class, IRequest<TResponse>
        {
            LogBefore(handler, request);
            var response = await handler.HandleAsync(request, cancellationToken);
            LogAfter(handler, request);
            return response;
        }

        private void LogBefore(IHandler handler, IMessage message)
        {
            Trace.WriteLine($"Using {handler.GetType().Name} to handle {message.GetType().Name}");
        }

        private void LogAfter(IHandler handler, IMessage message)
        {
            Trace.WriteLine($"Used {handler.GetType().Name} to handle {message.GetType().Name}");
        }
    }
}