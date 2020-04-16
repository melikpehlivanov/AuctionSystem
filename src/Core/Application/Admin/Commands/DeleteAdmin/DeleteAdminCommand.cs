namespace Application.Admin.Commands.DeleteAdmin
{
    using MediatR;

    public class DeleteAdminCommand : IRequest
    {
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
