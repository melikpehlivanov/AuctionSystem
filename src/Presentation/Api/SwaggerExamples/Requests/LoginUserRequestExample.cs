namespace Api.SwaggerExamples.Requests
{
    using Models.Users;
    using Swashbuckle.AspNetCore.Filters;

    public class LoginUserRequestExample : IExamplesProvider<LoginUserRequestModel>
    {
        public LoginUserRequestModel GetExamples()
            => new LoginUserRequestModel { Email = "test@test.com", Password = "test123" };
    }
}
