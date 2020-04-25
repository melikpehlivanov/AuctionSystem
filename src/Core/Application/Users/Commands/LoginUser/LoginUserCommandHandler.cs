namespace Application.Users.Commands.LoginUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using Jwt;
    using MediatR;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<LoginUserResponseModel>>
    {
        private readonly IMediator mediator;
        private readonly IUserManager userManager;

        public LoginUserCommandHandler(IUserManager userManager, IMediator mediator)
        {
            this.userManager = userManager;
            this.mediator = mediator;
        }

        public async Task<Response<LoginUserResponseModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var (result, userId) = await this.userManager.CheckCredentials(request.Email, request.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException(ExceptionMessages.User.InvalidCredentials);
            }

            var jwtToken = await this.mediator
                .Send(new GenerateJwtTokenCommand(userId, request.Email), cancellationToken);
            return new Response<LoginUserResponseModel>(new LoginUserResponseModel { Token = jwtToken });
        }
    }
}