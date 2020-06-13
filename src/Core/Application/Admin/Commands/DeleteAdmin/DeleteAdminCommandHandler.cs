namespace Application.Admin.Commands.DeleteAdmin
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using MediatR;

    public class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUserService currentUserService;

        public DeleteAdminCommandHandler(IUserManager userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
        {
            //TODO: Add User as default role in db
            if (!request.Role.Equals(AppConstants.AdministratorRole))
            {
                throw new BadRequestException(ExceptionMessages.Admin.InvalidRole);
            }

            var result =
                await this.userManager.RemoveFromRoleAsync(request.Email, request.Role, this.currentUserService.UserId);
            if (!result.identityResult.Succeeded)
            {
                throw new BadRequestException(result.errorMessage);
            }

            return Unit.Value;
        }
    }
}