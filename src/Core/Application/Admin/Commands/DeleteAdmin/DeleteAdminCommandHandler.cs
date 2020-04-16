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

        public DeleteAdminCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
        {
            //TODO: Add User as default role in db
            if (!request.Role.Equals(AppConstants.AdministratorRole))
            {
                throw new BadRequestException(ExceptionMessages.Admin.InvalidRole);
            }

            var result = await this.userManager.RemoveFromRoleAsync(request.Email, request.Role);
            if (!result)
            {
                throw new BadRequestException(string.Format(
                    ExceptionMessages.Admin.UserNotRemovedSuccessfullyFromRole, request.Role));
            }

            return Unit.Value;
        }
    }
}