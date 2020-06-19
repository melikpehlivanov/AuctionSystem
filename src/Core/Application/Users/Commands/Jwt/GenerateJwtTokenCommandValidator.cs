namespace Application.Users.Commands.Jwt
{
    using FluentValidation;

    public class GenerateJwtTokenCommandValidator : AbstractValidator<GenerateJwtTokenCommand>
    {
        public GenerateJwtTokenCommandValidator()
        {
            this.RuleFor(p => p.UserId).NotEmpty();
            this.RuleFor(p => p.Username).NotEmpty();
        }
    }
}