namespace Application.Admin.Commands.AddToRole
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using MediatR;

    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand>
    {
        private readonly IUserManager userManager;

        public AddUserToRoleCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        {
            //TODO: Add User as default role in db
            if (!request.Role.Equals(AppConstants.AdministratorRole))
            {
                throw new BadRequestException("Invalid role");
            }

            var result = await this.userManager.AddToRoleAsync(request.Email, request.Role);
            if (!result)
            {
                throw new BadRequestException($"Something went wrong while adding user to {request.Role} role");
            }

            return Unit.Value;
        }
    }
}
