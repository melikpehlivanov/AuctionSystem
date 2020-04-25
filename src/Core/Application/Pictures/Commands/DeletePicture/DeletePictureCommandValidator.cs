namespace Application.Pictures.Commands.DeletePicture
{
    using FluentValidation;

    public class DeletePictureCommandValidator : AbstractValidator<DeletePictureCommand>
    {
        public DeletePictureCommandValidator()
        {
            this.RuleFor(p => p.PictureId).NotEmpty();
            this.RuleFor(p => p.ItemId).NotEmpty();
        }
    }
}