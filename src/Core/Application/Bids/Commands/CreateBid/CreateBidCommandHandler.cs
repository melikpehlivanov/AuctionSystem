namespace Application.Bids.Commands.CreateBid
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using AutoMapper;
    using Common;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

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
                    b.ItemId
                })
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync(b => b.ItemId == request.ItemId, cancellationToken);
            if (request.Amount <= currentHighestBid.Amount)
            {
                throw new BadRequestException("Invalid bid amount");
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
                throw new NotFoundException(nameof(Item), request.ItemId);
            }

            if (request.UserId != this.currentUserService.UserId)
            {
                throw new NotFoundException(nameof(Item));
            }

            if (item.StartTime >= this.dateTime.UtcNow)
            {
                //Bid hasn't started yet.
                throw new BadRequestException($"Biding for item {request.ItemId} hasn't started yet");
            }

            if (item.EndTime <= this.dateTime.UtcNow)
            {
                //Bidding has ended
                throw new BadRequestException($"Bidding for item {request.ItemId} have ended.");
            }
        }
    }
}
