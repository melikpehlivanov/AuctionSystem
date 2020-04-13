namespace Application.Users.Commands.CreateUser
{
    using FluentValidation;
    using global::Common;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            this.RuleFor(p => p.Email).NotNull().Matches(ModelConstants.User.EmailRegex);
            this.RuleFor(p => p.FullName).NotNull().MaximumLength(ModelConstants.User.FullNameMaxLength);
            this.RuleFor(p => p.Password).NotNull();
        }
    }
}
