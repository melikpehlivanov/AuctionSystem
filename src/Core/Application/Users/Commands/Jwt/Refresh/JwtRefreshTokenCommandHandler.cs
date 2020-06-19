namespace Application.Users.Commands.Jwt.Refresh
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Models;
    using AppSettingsModels;
    using Common.Exceptions;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class JwtRefreshTokenCommandHandler :
        BaseJwtTokenHandler,
        IRequestHandler<JwtRefreshTokenCommand, Response<AuthSuccessResponse>>
    {
        private readonly TokenValidationParameters tokenValidationParameters;

        public JwtRefreshTokenCommandHandler(
            IOptions<JwtSettings> options,
            IUserManager userManager,
            IAuctionSystemDbContext context,
            TokenValidationParameters tokenValidationParameters)
            : base(options, userManager, context)
        {
            this.tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<Response<AuthSuccessResponse>> Handle(JwtRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var validatedToken = this.GetPrincipalFromToken(request.Token);

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await this.Context
                .RefreshTokens
                .SingleOrDefaultAsync(t => t.Token == request.RefreshToken, cancellationToken);

            if (validatedToken == null
                || expiryDateTimeUtc > DateTime.UtcNow
                || storedRefreshToken == null
                || DateTime.UtcNow > storedRefreshToken.ExpiryDate
                || storedRefreshToken.Invalidated
                || storedRefreshToken.Used
                || storedRefreshToken.JwtId != jti)
            {
                throw new BadRequestException(ExceptionMessages.User.InvalidRefreshToken);
            }

            this.Context.RefreshTokens.Remove(storedRefreshToken);
            await this.Context.SaveChangesAsync(cancellationToken);

            var userId = validatedToken.Claims.Single(x => x.Type == "id").Value;
            var user = await this.UserManager.GetUserByIdAsync(userId);

            return new Response<AuthSuccessResponse>(await this.GenerateAuthResponse(user.Id, user.UserName, cancellationToken));
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var clonedTokenValidationParameters = this.tokenValidationParameters.Clone();
                clonedTokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, clonedTokenValidationParameters, out var validatedToken);
                return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
            }
            catch
            {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken token)
            => (token is JwtSecurityToken jwtSecurityToken)
               && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }
}