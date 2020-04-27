namespace Application.Users.Commands.Jwt.Refresh
{
    using FluentValidation;

    public class JwtRefreshTokenCommandValidator : AbstractValidator<JwtRefreshTokenCommand>
    {
        public JwtRefreshTokenCommandValidator()
        {
            this.RuleFor(p => p.Token).NotEmpty();
            this.RuleFor(p => p.RefreshToken).NotEmpty();
        }
    }
}
