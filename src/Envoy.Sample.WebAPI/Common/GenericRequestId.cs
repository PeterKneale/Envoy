namespace Envoy.Sample.WebAPI.Controllers
{
    public class GenericRequestId<T>
    {
        public GenericRequestId(T payload)
        {
            Id = payload;
        }

        public T Id { get; }
    }
}
