namespace Application.Users.Commands.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.Exceptions;
    using Common.Interfaces;
    using MediatR;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IUserManager userManager;
        private readonly IEmailSender emailSender;

        public CreateUserCommandHandler(IUserManager userManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await this.userManager.CreateUserAsync(request.Email, request.Password, request.FullName);
            if (!result.Succeeded)
            {
                throw new BadRequestException(ExceptionMessages.User.UserNotCreatedSuccessfully);
            }

            var token = await this.userManager.GenerateEmailConfirmationCode(request.Email);
            await this.emailSender.SendConfirmationEmail(request.Email, token);

            return Unit.Value;
        }
    }
}