namespace Application.Items.Commands.UpdateItem
{
    using FluentValidation;
    using global::Common;

    public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        private readonly IDateTime dateTime;

        public UpdateItemCommandValidator(IDateTime dateTime)
        {
            this.dateTime = dateTime;

            this.RuleFor(p => p.Id).NotEmpty();
            this.RuleFor(p => p.Title).NotEmpty().MaximumLength(ModelConstants.Item.TitleMaxLength);
            this.RuleFor(p => p.Description).NotEmpty().MaximumLength(ModelConstants.Item.DescriptionMaxLength);
            this.RuleFor(p => p.StartingPrice).NotEmpty()
                .InclusiveBetween(ModelConstants.Item.MinStartingPrice, ModelConstants.Item.MaxStartingPrice);
            this.RuleFor(p => p.MinIncrease).NotEmpty()
                .InclusiveBetween(ModelConstants.Item.MinMinIncrease, ModelConstants.Item.MaxMinIncrease);

            this.RuleFor(p => p.StartTime).NotEmpty();
            this.RuleFor(p => p.EndTime).NotEmpty();

            this.RuleFor(m => new { m.StartTime, m.EndTime }).NotEmpty()
                .Must(x => x.EndTime.Date.ToUniversalTime() >= x.StartTime.Date.ToUniversalTime())
                .WithMessage("End time must be after start time")
                .Must(x => x.StartTime.ToUniversalTime() >= this.dateTime.Now.ToUniversalTime())
                .WithMessage("The Start time must be after the current time");

            this.RuleFor(p => p.SubCategoryId).NotEmpty();
        }
    }
}