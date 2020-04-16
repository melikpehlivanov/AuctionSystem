namespace Application.Users.Commands.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using MediatR;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IUserManager userManager;

        public CreateUserCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await this.userManager.CreateUserAsync(request.Email, request.Password, request.FullName);
            if (!result.Succeeded)
            {
                throw new BadRequestException("User was not created successfully");
            }

            return Unit.Value;
        }
    }
}