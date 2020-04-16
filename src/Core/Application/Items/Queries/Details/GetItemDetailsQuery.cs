namespace Application.Items.Queries.Details
{
    using System;
    using Common.Models;
    using MediatR;

    public class GetItemDetailsQuery : IRequest<Response<ItemDetailsResponseModel>>
    {
        public GetItemDetailsQuery(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }
}
