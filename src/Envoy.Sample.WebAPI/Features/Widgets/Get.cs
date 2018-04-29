using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy.Sample.WebAPI.Controllers
{
    public class Get
    {
        public class Request : IRequest<Response>
        {
            public Request(int id) 
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Response
        {
            public Response(Widget widget) 
            {
                Widget = widget;
            }

            public Widget Widget { get; }
        }

        public class Handler : IHandleRequest<Request, Response>
        {
            public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var widget = Database.Widgets.SingleOrDefault(x => x.Id == request.Id);
                return await Task.FromResult(new Response(widget));
            }
        }
    }
}
