namespace Application.Bids.Queries.Details
{
    using System;
    using Common.Models;
    using MediatR;

    public class GetHighestBidDetailsQuery : IRequest<Response<GetHighestBidDetailsResponseModel>>
    {
        public GetHighestBidDetailsQuery(Guid itemId)
        {
            this.ItemId = itemId;
        }

        public Guid ItemId { get; }
    }
}
