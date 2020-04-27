namespace Application.Users.Commands.Jwt.Refresh
{
    using System;
    using Common.Models;
    using MediatR;

    public class JwtRefreshTokenCommand : IRequest<Response<AuthSuccessResponse>>
    {
        public string Token { get; set; }

        public Guid RefreshToken { get; set; }
    }
}
