using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Envoy.Sample.WebAPI.Controllers
{
    [Route("api/widgets")]
    public class API : Controller
    {
        private readonly IDispatchRequests _requests;
        private readonly IDispatchCommands _commands;
        private readonly IDispatchEvents _events;

        public API(IDispatchRequests requests, IDispatchCommands commands, IDispatchEvents events)
        {
            _requests = requests;
            _commands = commands;
            _events = events;
        }

        // GET api/widgets
        [HttpGet]
        public async Task<IEnumerable<Widget>> Get()
        {
            var result = await _requests.RequestAsync<List.Request, List.Response>(new List.Request());
            return result.Widgets;
        }

        // GET api/widgets/5
        [HttpGet("{id}")]
        public async Task<Widget> Get(int id)
        {
            var result = await _requests.RequestAsync<Get.Request, Get.Response>(new Get.Request(id));
            return result.Widget;
        }

        // POST api/widgets/5
        [HttpPost("{id}")]
        public async Task Post(int id, [FromBody]CreateModel model)
        {
            await _commands.CommandAsync(new Create.Command(id, model.Name));
        }

        // PUT api/widgets/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/widgets/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _commands.CommandAsync(new Delete.Command(id));
            await _events.PublishAsync(new WidgetDeletedEvent());
        }
    }
    public class CreateModel
    {
        public string Name { get; set; }
    }
}
