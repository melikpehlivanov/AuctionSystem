namespace Application.Users.Commands.CreateUser
{
    using FluentValidation;
    using global::Common;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p => p.Email).NotNull().Matches(ModelConstants.User.EmailRegex);
            RuleFor(p => p.FullName).NotNull().MaximumLength(ModelConstants.User.FullNameMaxLength);
            RuleFor(p => p.Password).NotNull();
        }
    }
}
