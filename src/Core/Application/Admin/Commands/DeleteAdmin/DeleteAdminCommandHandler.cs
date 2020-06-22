namespace Application.Admin.Commands.DeleteAdmin
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using MediatR;

    public class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IUserManager userManager;

        public DeleteAdminCommandHandler(ICurrentUserService currentUserService, IUserManager userManager)
        {
            this.currentUserService = currentUserService;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
        {
            //TODO: Add User as default role in db
            if (!request.Role.Equals(AppConstants.AdministratorRole))
            {
                throw new BadRequestException(ExceptionMessages.Admin.InvalidRole);
            }

            var result =
                await this.userManager.RemoveFromRoleAsync(request.Email, request.Role, this.currentUserService.UserId,
                    cancellationToken);
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