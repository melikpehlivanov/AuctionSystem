namespace Api.SwaggerExamples.Responses
{
    using Swashbuckle.AspNetCore.Filters;

    public class BadRequestResponseModel : IExamplesProvider<BadRequestErrorModel>
    {
        public BadRequestErrorModel GetExamples()
            => new BadRequestErrorModel
            {
                Error = "An error occured while creating/deleting/updating given entity(User, Item and etc.)",
                Title = "BadRequest",
                Status = 400, 
                TraceId = "8000006c-0007-ff00-b63f-84710c7967bb"
            };
    }
}
