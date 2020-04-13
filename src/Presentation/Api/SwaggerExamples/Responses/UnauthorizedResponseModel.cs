namespace Api.SwaggerExamples.Responses
{
    using Models;
    using Swashbuckle.AspNetCore.Filters;

    public class UnauthorizedResponse : IExamplesProvider<ErrorModel>
    {
        public ErrorModel GetExamples()
            => new ErrorModel
            {
                Error = "Log-in required", 
                Status = 401, 
                Title = "BadRequest", 
                TraceId = "8000006c-0007-ff00-b63f-84710c7967bb"
            };
    }
}
