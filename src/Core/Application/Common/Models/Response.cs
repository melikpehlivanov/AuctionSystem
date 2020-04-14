namespace Application.Common.Models
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(T response)
        {
            this.Data = response;
        }

        public T Data { get; }
    }
}
