namespace Application.Users.Commands.LoginUser
{
    using FluentValidation;
    using FluentValidation.Validators;
    using global::Common;

    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            this.RuleFor(p => p.Email).NotNull().EmailAddress(EmailValidationMode.Net4xRegex);
            this.RuleFor(p => p.Password).NotNull();
        }
    }
}
