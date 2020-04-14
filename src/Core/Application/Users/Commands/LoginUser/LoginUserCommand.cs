namespace Application.Users.Commands.LoginUser
{
    using Common.Models;
    using MediatR;

    public class LoginUserCommand : IRequest<Response<LoginUserResponseModel>>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
