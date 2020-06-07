namespace Application.Pictures.Commands.UpdatePicture
{
    using FluentValidation;
    using Items.Commands.UpdateItem;

    public class UpdatePictureCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdatePictureCommandValidator()
        {
            this.RuleFor(p => p.Id).NotEmpty();
        }
    }
}