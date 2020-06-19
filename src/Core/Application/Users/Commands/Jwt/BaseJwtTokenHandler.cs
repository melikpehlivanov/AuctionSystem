namespace Application.Users.Commands.Jwt
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AppSettingsModels;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public abstract class BaseJwtTokenHandler
    {
        private readonly JwtSettings options;
        protected readonly IUserManager UserManager;
        protected readonly IAuctionSystemDbContext Context;
        protected readonly IDateTime DateTime;

        protected BaseJwtTokenHandler(
            IOptions<JwtSettings> options,
            IUserManager userManager,
            IAuctionSystemDbContext context,
            IDateTime dateTime)
        {
            this.UserManager = userManager;
            this.Context = context;
            this.DateTime = dateTime;
            this.options = options.Value;
        }

        protected async Task<AuthSuccessResponse> GenerateAuthResponse(string userId, string userName,
            CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.options.Secret);

            var userRoles = await this.UserManager.GetUserRolesAsync(userId);
            var claims = new ClaimsIdentity(new[]
            {
                new Claim("id", userId),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, userName),
            });

            foreach (var role in userRoles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = this.DateTime.UtcNow.Add(this.options.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = userId,
                CreationDate = this.DateTime.UtcNow,
                ExpiryDate = this.DateTime.UtcNow.AddMonths(AppConstants.RefreshTokenExpirationTimeInMonths),
            };

            await this.Context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await this.Context.SaveChangesAsync(cancellationToken);

            var result = new AuthSuccessResponse
            {
                Token = encryptedToken,
                RefreshToken = refreshToken.Token,
            };

            return result;
        }
    }
}