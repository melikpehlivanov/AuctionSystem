namespace Application.Users.Commands.CreateUser
{
    using FluentValidation;
    using global::Common;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            this.RuleFor(p => p.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
            this.RuleFor(p => p.FullName).NotEmpty().MaximumLength(ModelConstants.User.FullNameMaxLength);
            this.RuleFor(p => p.Password).NotEmpty();
        }
    }
}