namespace Application.Items.Queries.Details
{
    using System;
    using MediatR;

    public class GetItemDetailsRequest : IRequest<ItemDetailsResponseModel>
    {
        public GetItemDetailsRequest(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }
}
