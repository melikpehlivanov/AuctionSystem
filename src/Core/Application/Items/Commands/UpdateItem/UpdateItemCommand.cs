namespace Application.Items.Commands.UpdateItem
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class UpdateItemCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid SubCategoryId { get; set; }

        public ICollection<IFormFile> PicturesToAdd { get; set; } = new HashSet<IFormFile>();

        public ICollection<Guid> PicturesToRemove { get; set; } = new HashSet<Guid>();
    }
}