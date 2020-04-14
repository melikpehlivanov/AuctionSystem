namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Application.Common.Models;
    using Application.Pictures;
    using Application.Pictures.Commands.CreatePicture;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Authorize]
    public class PicturesController : BaseController
    {
        /// <summary>
        /// Uploads pictures
        /// </summary>
        /// <returns>Collection of the corresponding pictures with their Id's and urls</returns>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status200OK, 
            "Images were uploaded successfully and their corresponding data is returned",
            typeof(Response<PictureResponseModel>))]
        public async Task<IActionResult> Post(CreatePictureCommand model)
        {
            var result = await this.Mediator.Send(model);
            return Ok(result);
        }
    }
}
