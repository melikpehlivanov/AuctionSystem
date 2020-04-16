namespace MvcWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Items.Queries.Details;
    using Application.Pictures;
    using Application.Pictures.Commands.CreatePicture;
    using Application.Pictures.Commands.DeletePicture;
    using Application.Pictures.Queries;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PicturesController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> UploadPictures(Guid id)
        {
            try
            {
                var item = await this.Mediator.Send(new GetItemDetailsQuery(id));
                var response = await this.Mediator.Send(new CreatePictureCommand
                {
                    ItemId = item.Data.Id,
                    Pictures = (ICollection<IFormFile>)this.Request.Form.Files
                });

                var pictures = (IEnumerable<PictureResponseModel>)response.Data;
                var urls = pictures.Select(p => p.Url).ToList();
                return this.Json(new { urls });
            }
            catch (NotFoundException)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.NotFound();
            }
            catch (ValidationException)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePictures(Guid pictureId)
        {
            try
            {
                var picture = await this.Mediator.Send(new GetPictureDetailsQuery(pictureId));
                await this.Mediator.Send(new DeletePictureCommand { ItemId = picture.Data.FirstOrDefault().ItemId, PictureId = pictureId });
                return this.Json(new object());
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
            catch (ValidationException)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.BadRequest();
            }
        }
    }
}
