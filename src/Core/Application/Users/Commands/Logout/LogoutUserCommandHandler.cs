namespace Application.Users.Commands.Logout
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Unit>
    {
        private readonly IAuctionSystemDbContext context;

        public LogoutUserCommandHandler(IAuctionSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            // If we don't have refresh token it means that user is already logged out
            if (request.RefreshToken == null)
            {
                return Unit.Value;
            }
            
            var refreshToken = await this.context
                .RefreshTokens
                .Where(r => r.Token == Guid.Parse(request.RefreshToken))
                .SingleOrDefaultAsync(cancellationToken);
            refreshToken.Invalidated = true;

            this.context.RefreshTokens.Update(refreshToken);
            await this.context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}