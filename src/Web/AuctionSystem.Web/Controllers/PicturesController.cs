namespace AuctionSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using Services.Models.Item;
    using Services.Models.Picture;

    [Authorize]
    public class PicturesController : BaseController
    {
        private readonly IPictureService pictureService;
        private readonly IItemsService ItemsService;

        public PicturesController(IPictureService pictureService, IItemsService itemsService)
        {
            this.pictureService = pictureService;
            this.ItemsService = itemsService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPictures(string id, string title)
        {
            try
            {
                var serviceItem = await this.ItemsService.GetByIdAsync<ItemDetailsServiceModel>(id);
                if (serviceItem == null || serviceItem.UserUserName != this.User.Identity.Name &&
                    !this.User.IsInRole(WebConstants.AdministratorRole))
                {
                    return this.NotFound();
                }
                var uploads = await this.pictureService.Upload((ICollection<IFormFile>)this.Request.Form.Files, id, title);
                var urls = uploads.Select(p => p.SecureUri.AbsoluteUri).ToList();
                return this.Json(new { urls });
            }
            catch (Exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePictures(string pictureId)
        {
            var extension = $".{pictureId.Split('.').LastOrDefault()?.ToLower()}";
            var pictureIdWithoutExtension = pictureId.Remove(pictureId.LastIndexOf(extension, StringComparison.Ordinal));
            var picture = await this.pictureService.GetPictureById<PictureDeleteServiceModel>(pictureIdWithoutExtension);
            if (picture == null)
            {
                return this.NotFound();
            }
            var serviceItem = await this.ItemsService.GetByIdAsync<ItemDetailsServiceModel>(picture.ItemId);
            if (serviceItem == null || serviceItem.UserUserName != this.User.Identity.Name &&
                !this.User.IsInRole(WebConstants.AdministratorRole))
            {
                return this.NotFound();
            }
            await this.pictureService.Delete(picture.ItemTitle, picture.ItemId, picture.Id);

            return this.Ok();
        }
    }
}
