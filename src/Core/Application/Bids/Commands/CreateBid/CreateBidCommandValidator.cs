namespace Application.Bids.Commands.CreateBid
{
    using FluentValidation;
    using global::Common;

    public class CreateBidCommandValidator : AbstractValidator<CreateBidCommand>
    {
        public CreateBidCommandValidator()
        {
            this.RuleFor(p => p.Amount).NotEmpty()
                .InclusiveBetween(ModelConstants.Bid.MinAmount, ModelConstants.Bid.MaxAmount);
            this.RuleFor(p => p.ItemId).NotEmpty();
            this.RuleFor(p => p.UserId).NotEmpty();
        }
    }
}