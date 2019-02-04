namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public interface IPictureService
    {
        IEnumerable<ImageUploadResult> Upload(ICollection<IFormFile> pictures, string itemId, string title);

        void Delete(string itemTitle);
    }
}
