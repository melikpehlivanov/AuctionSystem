namespace Api.SwaggerExamples.Requests
{
    using Models.Users;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateUserRequestExample : IExamplesProvider<CreateUserRequestModel>
    {
        public CreateUserRequestModel GetExamples()
            => new CreateUserRequestModel { Email = "test@test.com", FullName = "Melik Pehlivanov", Password = "Test123" };
    }
}
