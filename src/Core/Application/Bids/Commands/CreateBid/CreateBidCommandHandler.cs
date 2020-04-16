namespace Application.Bids.Commands.CreateBid
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Exceptions;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;
        private readonly IMapper mapper;

        public CreateBidCommandHandler(
            IAuctionSystemDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IDateTime dateTime)
        {
            this.context = context;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            await this.CheckWhetherItemIsEligibleForBidding(request, cancellationToken);
            await this.CheckWhetherBidIsValid(request, cancellationToken);

            var bid = this.mapper.Map<Bid>(request);
            await this.context.Bids.AddAsync(bid, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task CheckWhetherBidIsValid(CreateBidCommand request, CancellationToken cancellationToken)
        {
            var currentHighestBid = await this.context
                .Bids
                .Select(b => new
                {
                    b.Amount,
                    b.ItemId,
                    StartingPrice = b.Item.StartingPrice,
                })
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync(b => b.ItemId == request.ItemId, cancellationToken);
            if (request.Amount <= currentHighestBid?.Amount 
                || request.Amount <= currentHighestBid?.StartingPrice)
            {
                throw new BadRequestException(ExceptionMessages.Bid.InvalidBidAmount);
            }
        }

        private async Task CheckWhetherItemIsEligibleForBidding(CreateBidCommand request, CancellationToken cancellationToken)
        {
            var item = await this.context
                .Items
                .Where(i => i.Id == request.ItemId)
                .SingleOrDefaultAsync(cancellationToken);

            if (item == null)
            {
                throw new NotFoundException(nameof(Item));
            }

            if (request.UserId != this.currentUserService.UserId)
            {
                throw new NotFoundException(nameof(Item));
            }

            if (item.StartTime >= this.dateTime.UtcNow)
            {
                //Bid hasn't started yet.
                throw new BadRequestException(
                    string.Format(ExceptionMessages.Bid.BiddingNotStartedYet, request.ItemId));
            }

            if (item.EndTime <= this.dateTime.UtcNow)
            {
                // Bidding has ended
                throw new BadRequestException(
                    string.Format(ExceptionMessages.Bid.BiddingHasEnded, request.ItemId));
            }
        }
    }
}