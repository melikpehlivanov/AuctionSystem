namespace Application.Users.Commands.CreateUser
{
    using FluentValidation;
    using FluentValidation.Validators;
    using global::Common;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            this.RuleFor(p => p.Email).NotNull().EmailAddress(EmailValidationMode.Net4xRegex);
            this.RuleFor(p => p.FullName).NotNull().MaximumLength(ModelConstants.User.FullNameMaxLength);
            this.RuleFor(p => p.Password).NotNull();
        }
    }
}
