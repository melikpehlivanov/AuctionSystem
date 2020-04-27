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
    using MediatR;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class GenerateJwtTokenCommandHandler : IRequestHandler<GenerateJwtTokenCommand, string>
    {
        private const int TokenExpiryDateInDays = 7;

        private readonly JwtSettings options;
        private readonly IUserManager userManager;

        public GenerateJwtTokenCommandHandler(IOptions<JwtSettings> options, IUserManager userManager)
        {
            this.userManager = userManager;
            this.options = options.Value;
        }

        public async Task<string> Handle(GenerateJwtTokenCommand request, CancellationToken cancellationToken)
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
                Expires = DateTime.UtcNow.AddDays(TokenExpiryDateInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
    }
}