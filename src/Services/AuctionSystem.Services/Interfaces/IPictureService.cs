namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    public interface IPictureService
    {
        Task<IEnumerable<UploadResult>> Upload(ICollection<Stream> pictureStreams, string itemId);

        Task DeleteItemFolder(string itemId);
        Task Delete(string itemId, string pictureId);
        Task<T> GetPictureById<T>(string pictureId);
    }
}
