namespace Api.SwaggerExamples.Requests
{
    using Application.Admin.Commands.DeleteAdmin;
    using Swashbuckle.AspNetCore.Filters;

    public class DeleteAdminRequestExample : IExamplesProvider<DeleteAdminCommand>
    {
        public DeleteAdminCommand GetExamples()
            => new DeleteAdminCommand { Email = "admin@admin.com", Role = "Administrator" };
    }
}
