namespace Application.Users.Commands.CreateUser
{
    using MediatR;

    public class CreateUserCommand : IRequest
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }
    }
}