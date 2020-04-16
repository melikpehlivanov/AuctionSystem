namespace Application.Admin.Commands.AddToRole
{
    using Common;
    using FluentValidation;
    using global::Common;

    public class AddUserToRoleCommandValidator : AbstractValidator<AddUserToRoleCommand>
    {
        public AddUserToRoleCommandValidator()
        {
            this.RuleFor(u => u.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
            this.RuleFor(u => u.Role).NotEmpty();
        }
    }
}
