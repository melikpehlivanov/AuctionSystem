namespace Api.SwaggerExamples.Responses
{
    using Models.Errors;
    using Swashbuckle.AspNetCore.Filters;

    public class UnauthorizedResponseModel : IExamplesProvider<BaseErrorModel>
    {
        public BaseErrorModel GetExamples()
            => new ErrorModel
            {
                Error = "Log-in required", 
                Status = 401, 
                Title = "Unauthorized", 
                TraceId = "8000006c-0007-ff00-b63f-84710c7967bb"
            };
    }
}
