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

            this.RuleFor(p => p.Id).NotNull();
            this.RuleFor(p => p.Title).NotNull().MaximumLength(ModelConstants.Item.TitleMaxLength);
            this.RuleFor(p => p.Description).NotNull().MaximumLength(ModelConstants.Item.DescriptionMaxLength);
            this.RuleFor(p => p.StartingPrice).NotNull()
                .InclusiveBetween(ModelConstants.Item.MinStartingPrice, ModelConstants.Item.MaxStartingPrice);
            this.RuleFor(p => p.MinIncrease).NotNull()
                .InclusiveBetween(ModelConstants.Item.MinMinIncrease, ModelConstants.Item.MaxMinIncrease);

            this.RuleFor(p => p.StartTime).NotNull();
            this.RuleFor(p => p.EndTime).NotNull();

            this.RuleFor(m => new { m.StartTime, m.EndTime }).NotNull()
                .Must(x => x.EndTime.Date.ToUniversalTime() >= x.StartTime.Date.ToUniversalTime())
                .WithMessage("End time must be after start time")
                .Must(x => x.StartTime.ToUniversalTime() >= this.dateTime.Now.ToUniversalTime())
                .WithMessage("The Start time must be after the current time");

            this.RuleFor(p => p.SubCategoryId).NotNull();
        }
    }
}
