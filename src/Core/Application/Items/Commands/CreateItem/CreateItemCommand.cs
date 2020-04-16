namespace Application.Items.Commands.CreateItem
{
    using System;
    using System.Collections.Generic;
    using Common.Models;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class CreateItemCommand : IRequest<Response<ItemResponseModel>>, IMapWith<Item>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid SubCategoryId { get; set; }

        public ICollection<IFormFile> PictureFormFiles { get; set; } = new HashSet<IFormFile>();
    }
}
