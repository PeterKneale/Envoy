using System.Threading;
using System.Threading.Tasks;

namespace Envoy.Sample.WebAPI.Controllers
{
    public class Delete
    {
        public class Command :ICommand
        {
            public Command(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Handler : IHandleCommand<Command>
        {
            public Task Handle(Command command, CancellationToken cancellationToken)
            {
                Database.Widgets.RemoveAll(x=>x.Id == command.Id);
                return Task.FromResult(0);
            }
        }
    }
}
