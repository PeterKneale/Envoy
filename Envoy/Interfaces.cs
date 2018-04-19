using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy
{
    public interface IMessage { }
    public interface IEvent : IMessage { }
    public interface ICommand : IMessage { }
    public interface IRequest<out TResponse> : IMessage { }

    public interface IDispatchEvent
    {
        Task PublishAsync<T>(T evnt, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent;
    }

    public interface IDispatchCommand
    {
        Task CommandAsync<T>(T command, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ICommand;
    }

    public interface IDispatchRequest
    {
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default(CancellationToken)) where TRequest : class, IRequest<TResponse>;
    }
    public interface IHandler
    {

    }
    public interface IHandleRequest<in TRequest, TResponse> : IHandler where TRequest : class, IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IHandleCommand<in ICommand> : IHandler where ICommand : class
    {
        Task HandleAsync(ICommand command, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IHandleEvent<in IEvent> : IHandler where IEvent : class
    {
        Task HandleAsync(IEvent evnt, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IExecuteCommands
    {
        Task ExecuteAsync<T>(IHandleCommand<T> handler, T command, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ICommand;

    }
    public interface IExecuteEvents
    {
        Task ExecuteAsync<T>(IEnumerable<IHandleEvent<T>> handlers, T evnt, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent;
    }

    public interface IExecuteRequests
    {
        Task<TResponse> Execute<TRequest, TResponse>(IHandleRequest<TRequest, TResponse> handler, TRequest request, CancellationToken cancellationToken = default(CancellationToken)) where TRequest : class, IRequest<TResponse>;
    }

    public interface IResolver
    {
        T Resolve<T>();
        IEnumerable<T> ResolveAll<T>();
    }

}