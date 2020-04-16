namespace Api.SwaggerExamples.Requests
{
    using Application.Admin.Commands.CreateAdmin;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateAdminRequestExample : IExamplesProvider<CreateAdminCommand>
    {
        public CreateAdminCommand GetExamples()
            => new CreateAdminCommand { Email = "test1@test.com", Role = "Administrator" };
    }
}