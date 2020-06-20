namespace Application.Users.Commands.ConfirmEmail
{
    using FluentValidation;
    using global::Common;

    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            this.RuleFor(p => p.Code).NotEmpty();
            this.RuleFor(p => p.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
        }
    }
}