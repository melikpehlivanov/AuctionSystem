namespace Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Models;
    using Application.Pictures;
    using Application.Pictures.Commands.CreatePicture;
    using Application.Pictures.Commands.DeletePicture;
    using Application.Pictures.Queries;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SwaggerExamples;
    using Swashbuckle.AspNetCore.Annotations;

    [Authorize]
    public class PicturesController : BaseController
    {
        /// <summary>
        /// Get details for given picture
        /// </summary>
        /// <returns>Returns the corresponding picture</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.PictureConstants.SuccessfulGetPictureDetailsRequestDescriptionMessage,
            typeof(MultiResponse<PictureDetailsResponseModel>))]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.PictureConstants.BadRequestDescriptionMessage,
            typeof(NotFoundErrorModel))]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await this.Mediator.Send(new GetPictureDetailsQuery(id));
            return this.Ok(result);
        }

        /// <summary>
        /// Uploads pictures
        /// </summary>
        /// <returns>Collection of the corresponding pictures with their Id's and urls</returns>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.PictureConstants.SuccessfulGetRequestDescriptionMessage,
            typeof(MultiResponse<PictureResponseModel>))]
        public async Task<IActionResult> Post([FromBody] CreatePictureCommand model)
        {
            var result = await this.Mediator.Send(model);
            return this.Ok(result);
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
                return this.BadRequest();
            }

            await this.Mediator.Send(model);
            return this.NoContent();
        }
    }
}