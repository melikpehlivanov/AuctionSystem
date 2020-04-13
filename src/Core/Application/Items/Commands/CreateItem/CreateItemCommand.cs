namespace Application.Items.Commands.CreateItem
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class CreateItemCommand : IRequest<Guid>, IMapWith<Item>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid SubCategoryId { get; set; }

        public string UserName { get; set; }

        public ICollection<IFormFile> Pictures { get; set; } = new HashSet<IFormFile>();
    }
}
