namespace Api.SwaggerExamples.Requests.Users
{
    using Application.Users.Commands.LoginUser;
    using Swashbuckle.AspNetCore.Filters;

    public class LoginUserRequestExample : IExamplesProvider<LoginUserCommand>
    {
        public LoginUserCommand GetExamples()
            => new LoginUserCommand { Email = "test@test.com", Password = "test123" };
    }
}