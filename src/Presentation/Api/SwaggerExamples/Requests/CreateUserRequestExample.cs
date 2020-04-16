namespace Api.SwaggerExamples.Requests
{
    using Application.Users.Commands.CreateUser;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateUserRequestExample : IExamplesProvider<CreateUserCommand>
    {
        public CreateUserCommand GetExamples()
            => new CreateUserCommand { Email = "test@test.com", FullName = "Melik Pehlivanov", Password = "Test123" };
    }
}