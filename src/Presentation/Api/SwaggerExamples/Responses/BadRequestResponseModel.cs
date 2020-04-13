namespace Api.SwaggerExamples.Responses
{
    using Models.Errors;
    using Swashbuckle.AspNetCore.Filters;

    public class BadRequestResponseModel : IExamplesProvider<BadRequestErrorModel>
    {
        public BadRequestErrorModel GetExamples()
            => new BadRequestErrorModel
            {
                Error = "Invalid user credentials",
                Title = "BadRequest",
                Status = 400, 
                TraceId = "8000006c-0007-ff00-b63f-84710c7967bb"
            };
    }
}
