namespace Application.Users.Commands.LoginUser
{
    using FluentValidation;
    using global::Common;

    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            this.RuleFor(p => p.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
            this.RuleFor(p => p.Password).NotEmpty();
        }
    }
}