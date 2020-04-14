namespace Application.Common.Models
{
    using System.Collections.Generic;

    public class Response<T>
    {
        private readonly T singleObjectData;
        private readonly IEnumerable<T> collectionData;

        public Response()
        {
        }

        public Response(T response)
        {
            this.singleObjectData = response;
        }

        public Response(IEnumerable<T> response)
        {
            this.collectionData = response;
        }

        public dynamic Data => this.singleObjectData != null ? (dynamic) this.singleObjectData : this.collectionData;
    }
}
