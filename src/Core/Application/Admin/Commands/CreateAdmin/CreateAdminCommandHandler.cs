namespace Application.Admin.Commands.CreateAdmin
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using MediatR;

    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUserService currentUserService;

        public CreateAdminCommandHandler(IUserManager userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
        {
            //TODO: Add User as default role in db
            if (!request.Role.Equals(AppConstants.AdministratorRole))
            {
                throw new BadRequestException(ExceptionMessages.Admin.InvalidRole);
            }

            var result =
                await this.userManager.AddToRoleAsync(request.Email, request.Role, this.currentUserService.UserId);
            if (!result.Succeeded && result.ErrorType == ErrorType.General)
            {
                throw new BadRequestException(result.Error);
            }

            if (!result.Succeeded && result.ErrorType == ErrorType.TokenExpired)
            {
                throw new UnauthorizedException();
            }

            return Unit.Value;
        }
    }
}