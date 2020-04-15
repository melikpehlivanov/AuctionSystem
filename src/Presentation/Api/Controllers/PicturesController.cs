namespace Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Models;
    using Application.Pictures;
    using Application.Pictures.Commands.CreatePicture;
    using Application.Pictures.Commands.DeletePicture;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SwaggerExamples;
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
            SwaggerDocumentation.PictureConstants.SuccessfulGetRequestDescriptionMessage,
            typeof(Response<PictureResponseModel>))]
        public async Task<IActionResult> Post([FromBody] CreatePictureCommand model)
        {
            var result = await this.Mediator.Send(model);
            return Ok(result);
        }

        /// <summary>
        /// Deletes picture
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent, 
            SwaggerDocumentation.PictureConstants.SuccessfulDeleteRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.PictureConstants.BadRequestDescriptionMessage,
            typeof(NotFoundErrorModel))]
        public async Task<IActionResult> Delete(Guid id, [FromBody] DeletePictureCommand model)
        {
            if (id != model.PictureId)
            {
                return BadRequest();
            }

            await this.Mediator.Send(model);
            return NoContent();
        }
    }
}
