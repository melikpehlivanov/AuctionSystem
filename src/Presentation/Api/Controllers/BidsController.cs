namespace Api.Controllers
{
    using System.Threading.Tasks;
    using SwaggerExamples;
    using Application.Bids.Commands.CreateBid;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Authorize]
    public class BidsController : BaseController
    {
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status204NoContent,
            SwaggerDocumentation.BidConstants.SuccessfulPostRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.BidConstants.BadRequestOnPostRequestDescriptionMessage,
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.BidConstants.UnauthorizedOnPostRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.BidConstants.NotFoundOnPostRequestDescriptionMessage,
            typeof(NotFoundErrorModel))]
        public async Task<IActionResult> Post([FromBody]CreateBidCommand model)
        {
            await this.Mediator.Send(model);
            return NoContent();
        }
    }
}
