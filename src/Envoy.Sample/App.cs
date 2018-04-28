namespace Envoy.Sample
{
    public interface IApp
    {
        void Start();
    }

    public class App : IApp
    {
        private readonly IDispatchCommands _commands;
        private readonly IDispatchEvents _events;
        private readonly IDispatchRequests _requests;

        public App(IDispatchCommands commands, IDispatchEvents events, IDispatchRequests requests)
        {
            _commands = commands;
            _events = events;
            _requests = requests;
        }

        public void Start()
        {
            _commands.CommandAsync(new TestCommand());
            _events.PublishAsync(new TestEvent());
            _requests.RequestAsync<TestRequest, TestResponse>(new TestRequest());
        }
    }
}
