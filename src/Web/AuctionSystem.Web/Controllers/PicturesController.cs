namespace AuctionSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using Services.Models.Picture;

    public class PicturesController : BaseController
    {
        private readonly IPictureService pictureService;

        public PicturesController(IPictureService pictureService)
        {
            this.pictureService = pictureService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPictures(string id, string title)
        {
            try
            {
                var uploads = await this.pictureService.Upload((ICollection<IFormFile>)this.Request.Form.Files, id, title);
                var urls = uploads.Select(p => p.SecureUri.AbsoluteUri).ToList();
                return this.Json(new { urls });
            }
            catch (Exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json("Upload failed");
            }
        }

        [HttpPost]
        public async Task DeletePictures(string pictureId)
        {
            var extension = $".{pictureId.Split('.').LastOrDefault()?.ToLower()}";
            var pictureIdWithoutExtension = pictureId.Remove(pictureId.LastIndexOf(extension, StringComparison.Ordinal));
            var picture = await this.pictureService.GetPictureById<PictureDeleteServiceModel>(pictureIdWithoutExtension);
            await this.pictureService.Delete(picture.ItemTitle, picture.ItemId, picture.Id);
        }
    }
}
