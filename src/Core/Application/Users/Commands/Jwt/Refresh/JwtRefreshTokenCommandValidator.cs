namespace Application.Users.Commands.Jwt.Refresh
{
    using FluentValidation;

    public class JwtRefreshTokenCommandValidator : AbstractValidator<JwtRefreshTokenCommand>
    {
        public JwtRefreshTokenCommandValidator()
        {
            this.RuleFor(p => p.Token).NotEmpty();
        }
    }
}
