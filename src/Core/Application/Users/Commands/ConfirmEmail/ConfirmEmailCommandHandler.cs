namespace Application.Users.Commands.ConfirmEmail
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using MediatR;

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IUserManager userManager;

        public ConfirmEmailCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await this.userManager.ConfirmEmail(request.Email, request.Code);
            if (!result)
            {
                throw new BadRequestException(ExceptionMessages.User.EmailVerificationFailed);
            }

            return Unit.Value;
        }
    }
}