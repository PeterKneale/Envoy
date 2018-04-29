using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy
{
    public class Executor : IExecuteCommands, IExecuteEvents, IExecuteRequests
    {
        public async Task ExecuteAsync<T>(
            IHandleCommand<T> handler, 
            T command, 
            CancellationToken cancellationToken = default(CancellationToken)) 
            where T : class, ICommand
        {
            await handler.Handle(command, cancellationToken);
        }

        public async Task ExecuteAsync<T>(
            IEnumerable<IHandleEvent<T>> handlers, 
            T evnt, 
            CancellationToken cancellationToken = default(CancellationToken)) 
            where T : class, IEvent
        {
            foreach(var handler in handlers)
            {
                await handler.HandleAsync(evnt, cancellationToken);
            }
        }
        
        public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(
            IHandleRequest<TRequest, TResponse> handler, 
            TRequest request, 
            CancellationToken cancellationToken = default(CancellationToken)) 
            where TRequest:class, IRequest<TResponse>
        {
            var response = await handler.HandleAsync(request, cancellationToken);
            return response;
        }
    }
}