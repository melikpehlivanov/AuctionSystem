namespace Api.SwaggerExamples.Responses
{
    using Models;
    using Swashbuckle.AspNetCore.Filters;

    public class BadRequestResponse : IExamplesProvider<ErrorModel>
    {
        public ErrorModel GetExamples()
            => new ErrorModel { Error = "Invalid user credentials", Status = 400, Title = "BadRequest", TraceId = "8000006c-0007-ff00-b63f-84710c7967bb" };
    }
}
