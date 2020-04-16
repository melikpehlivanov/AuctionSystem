namespace Application.Admin.Commands.CreateAdmin
{
    using FluentValidation;
    using global::Common;

    public class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
    {
        public CreateAdminCommandValidator()
        {
            this.RuleFor(u => u.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
            this.RuleFor(u => u.Role).NotEmpty();
        }
    }
}