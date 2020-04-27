namespace Application.Users.Commands.Jwt
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AppSettingsModels;
    using Common.Interfaces;
    using Domain.Entities;
    using MediatR;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class GenerateJwtTokenCommandHandler : IRequestHandler<GenerateJwtTokenCommand, AuthSuccessResponse>
    {
        private const int ExpiryDateInMonths = 6;

        private readonly JwtSettings options;
        private readonly IUserManager userManager;
        private readonly IAuctionSystemDbContext context;

        public GenerateJwtTokenCommandHandler(IOptions<JwtSettings> options, IUserManager userManager, IAuctionSystemDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
            this.options = options.Value;
        }

        public async Task<AuthSuccessResponse> Handle(GenerateJwtTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.options.Secret);

            var userRoles = await this.userManager.GetUserRolesAsync(request.UserId);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, request.UserId),
                new Claim(ClaimTypes.Name, request.Username)
            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(this.options.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                Token = token.Id,
                UserId = request.UserId,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(ExpiryDateInMonths),
            };

            await this.context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);

            var result = new AuthSuccessResponse
            {
                Token = encryptedToken,
                RefreshToken = refreshToken.Token,
            };
            return result;
        }
    }
}