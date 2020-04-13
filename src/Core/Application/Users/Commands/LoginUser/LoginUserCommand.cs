namespace Application.Users.Commands.LoginUser
{
    using MediatR;

    public class LoginUserCommand : IRequest<LoginUserResponseModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
