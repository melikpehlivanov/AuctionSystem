namespace Application.Items.Commands.DeleteItem
{
    using FluentValidation;

    public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
    {
        public DeleteItemCommandValidator()
        {
            this.RuleFor(p => p.Id).NotNull();
        }
    }
}