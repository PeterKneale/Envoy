namespace Envoy.Sample.WebAPI.Controllers
{
    public class GenericResponse<T>
    {
        public GenericResponse(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
