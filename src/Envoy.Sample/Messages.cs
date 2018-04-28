namespace Envoy.Sample
{
    public class TestCommand : ICommand { }

    public class TestEvent : IEvent { }

    public class TestRequest : IRequest<TestResponse> { }

    public class TestResponse { }
}
