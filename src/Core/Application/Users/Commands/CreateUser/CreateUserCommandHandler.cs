namespace Application.Users.Commands.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Common.Models;
    using MediatR;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IUserManager userManager;

        public CreateUserCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this.userManager.CreateUserAsync(request.Email, request.Password, request.FullName);
            return user.Result;
        }
    }
}
