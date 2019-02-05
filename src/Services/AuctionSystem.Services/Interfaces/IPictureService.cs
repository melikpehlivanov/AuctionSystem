namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public interface IPictureService
    {
        Task<IEnumerable<UploadResult>> Upload(ICollection<IFormFile> pictures, string itemId, string title);

        void Delete(string itemTitle, string itemId);
        Task Delete(string itemTitle, string itemId, string pictureId);
        Task<T> GetPictureById<T>(string pictureId);
    }
}
