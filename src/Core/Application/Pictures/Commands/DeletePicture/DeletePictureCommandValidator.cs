namespace Application.Pictures.Commands.DeletePicture
{
    using FluentValidation;

    public class DeletePictureCommandValidator : AbstractValidator<DeletePictureCommand>
    {
        public DeletePictureCommandValidator()
        {
            this.RuleFor(p => p.PictureId).NotNull();
            this.RuleFor(p => p.ItemId).NotNull();
        }
    }
}