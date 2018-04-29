using System.Threading;
using System.Threading.Tasks;

namespace Envoy.Sample.WebAPI.Controllers
{
    public class Create
    {
        public class Command : ICommand
        {
            public Command(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; }
            public string Name { get; }
        }

        public class Handler : IHandleCommand<Command>
        {
            public Task Handle(Command command, CancellationToken cancellationToken)
            {
                Database.Widgets.Add(new Widget { Id = command.Id, Name = command.Name });
                return Task.FromResult(0);
            }
        }
    }
}
