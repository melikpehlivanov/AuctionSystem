namespace Application.Users.Commands.Jwt
{
    using System.Threading;
    using System.Threading.Tasks;
    using AppSettingsModels;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Options;

    public class GenerateJwtTokenCommandHandler : BaseJwtTokenHandler ,IRequestHandler<GenerateJwtTokenCommand, AuthSuccessResponse>
    {
        public GenerateJwtTokenCommandHandler(
            IOptions<JwtSettings> options,
            IUserManager userManager,
            IAuctionSystemDbContext context)
            : base(options, userManager, context)
        {
        }

        public async Task<AuthSuccessResponse> Handle(GenerateJwtTokenCommand request, CancellationToken cancellationToken)
            => await this.GenerateAuthResponse(request.UserId, request.Username, cancellationToken);
    }
}