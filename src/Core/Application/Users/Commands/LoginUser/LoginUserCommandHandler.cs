namespace Application.Users.Commands.LoginUser
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using MediatR;
    using Microsoft.IdentityModel.Tokens;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponseModel>
    {
        private readonly IUserManager userManager;

        public LoginUserCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<LoginUserResponseModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this.userManager.GetUserByUsernameAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Email);
            }

            var (result, userId) = await this.userManager.CheckCredentials(request.Email, request.Password);
            if (!result.Succeeded)
            {
                throw new UnauthorizedException();
            }

            var jwtToken = this.GenerateJwtToken(userId, user.Email, request.Secret);
            return new LoginUserResponseModel { Token = jwtToken };
        }

        private string GenerateJwtToken(string userId, string userName, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
    }
}
