namespace Application.Items
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class ItemCreatedNotification : INotification
    {
        public ItemCreatedNotification(Guid itemId, ICollection<IFormFile> pictures)
        {
            this.ItemId = itemId;
            this.Pictures = pictures;
        }

        public Guid ItemId { get; }

        public ICollection<IFormFile> Pictures { get; }
    }
}
