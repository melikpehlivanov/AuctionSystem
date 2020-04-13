namespace Api.SwaggerExamples.Responses
{
    using Models;
    using Swashbuckle.AspNetCore.Filters;

    public class NotFoundResponse : IExamplesProvider<ErrorModel>
    {
        public ErrorModel GetExamples()
            => new ErrorModel
            {
                Error = "Such entity does not exist or this field could be empty", 
                Status = 404, 
                Title = "NotFound", 
                TraceId = "9000006b-0407-fb00-b63f-84710c8421cb"
            };
    }
}
