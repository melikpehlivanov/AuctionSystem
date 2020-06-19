namespace Application.Users.Commands.Jwt
{
    using System.Threading;
    using System.Threading.Tasks;
    using AppSettingsModels;
    using Common.Interfaces;
    using global::Common;
    using MediatR;
    using Microsoft.Extensions.Options;

    public class GenerateJwtTokenCommandHandler : BaseJwtTokenHandler,
        IRequestHandler<GenerateJwtTokenCommand, AuthSuccessResponse>
    {
        public GenerateJwtTokenCommandHandler(
            IOptions<JwtSettings> options,
            IUserManager userManager,
            IAuctionSystemDbContext context,
            IDateTime dateTime)
            : base(options, userManager, context, dateTime)
        {
        }

        public async Task<AuthSuccessResponse> Handle(GenerateJwtTokenCommand request,
            CancellationToken cancellationToken)
            => await this.GenerateAuthResponse(request.UserId, request.Username, cancellationToken);
    }
}