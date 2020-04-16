namespace Api.Controllers
{
    using System.Threading.Tasks;
    using SwaggerExamples;
    using Application.Bids.Commands.CreateBid;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Application.Bids.Queries.Details;
    using System;
    using Application.Common.Models;

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
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.BidConstants.NotFoundOnPostRequestDescriptionMessage,
            typeof(NotFoundErrorModel))]
        public async Task<IActionResult> Post([FromBody]CreateBidCommand model)
        {
            await this.Mediator.Send(model);
            return NoContent();
        }

        [HttpGet]
        [Route("getHighestBid/{itemId?}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.BidConstants.GetHighestBidDescriptionMessage,
            typeof(GetHighestBidDetailsResponseModel))]
        public async Task<IActionResult> GetHighestBid(Guid itemId)
        {
            var result = await this.Mediator.Send(new GetHighestBidDetailsQuery(itemId));
            return Ok(result);
        }
    }
}
