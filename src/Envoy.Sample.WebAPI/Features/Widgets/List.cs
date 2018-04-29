using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Envoy.Sample.WebAPI.Controllers
{
    public class List
    {
        public class Request :  IRequest<Response>
        {
        }

        public class Response
        {
            public Response(IEnumerable<Widget> widgets)
            {
                Widgets = widgets;
            }

            public IEnumerable<Widget> Widgets { get; }
        }

        public class Handler : IHandleRequest<Request, Response>
        {
            public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return await Task.FromResult(new Response(Database.Widgets));
            }
        }
    }
}
