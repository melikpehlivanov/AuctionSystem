namespace Application.Admin.Commands.CreateAdmin
{
    using MediatR;

    public class CreateAdminCommand : IRequest
    {
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
