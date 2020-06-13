namespace Application.Users.Commands.Logout
{
    using MediatR;

    public class LogoutUserCommand : IRequest
    {
        public string RefreshToken { get; set; }
    }
}