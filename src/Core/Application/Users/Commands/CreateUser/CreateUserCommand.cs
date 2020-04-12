namespace Application.Users.Commands.CreateUser
{
    using Common.Models;
    using MediatR;

    public class CreateUserCommand : IRequest<Result>
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }
    }
}
