namespace Application.Admin.Commands.AddToRole
{
    using MediatR;

    public class AddUserToRoleCommand : IRequest
    {
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
