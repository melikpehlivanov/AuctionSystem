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
        [SwaggerResponse(StatusCodes.Status204NoContent, "Bid is created successfully")]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest, 
            "Provided data is invalid",
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized, 
            "Indicates that user is not logged in")]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Indicates that such item does not exist.",
            typeof(NotFoundErrorModel))]
        public async Task<IActionResult> Post([FromBody]CreateBidCommand model)
        {
            await this.Mediator.Send(model);
            return NoContent();
        }
    }
}
