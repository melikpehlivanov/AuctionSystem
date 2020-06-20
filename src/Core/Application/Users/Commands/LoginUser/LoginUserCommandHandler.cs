namespace Application.Users.Commands.LoginUser
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using Jwt;
    using MediatR;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<AuthSuccessResponse>>
    {
        private readonly IMediator mediator;
        private readonly IUserManager userManager;
        private readonly IEmailSender emailSender;

        public LoginUserCommandHandler(IUserManager userManager, IMediator mediator, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.mediator = mediator;
            this.emailSender = emailSender;
        }

        public async Task<Response<AuthSuccessResponse>> Handle(LoginUserCommand request,
            CancellationToken cancellationToken)
        {
            var (result, userId) = await this.userManager.SignIn(request.Email, request.Password);
            if (!result.Succeeded && result.IsAccountConfirmationError)
            {
                var token = await this.userManager.GenerateEmailConfirmationCode(request.Email);
                await this.emailSender.SendConfirmationEmail(request.Email, token);
            }

            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.First());
            }

            var model = await this.mediator
                .Send(new GenerateJwtTokenCommand(userId, request.Email), cancellationToken);
            return new Response<AuthSuccessResponse>(model);
        }
    }
}