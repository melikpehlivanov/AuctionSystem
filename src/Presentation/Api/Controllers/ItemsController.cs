namespace Api.Controllers
{
    using System.Threading.Tasks;
    using SwaggerExamples;
    using Application.Items.Commands.CreateItem;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Authorize]
    public class ItemsController : BaseController
    {
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, "Creates item successfully and returns the Id of the item")]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest, 
            "Failed due to invalid data.", 
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized, 
            "Available only for authorized users", 
            typeof(UnauthorizedErrorModel))]
        public async Task<IActionResult> Post(CreateItemCommand model)
        {
            var result = await this.Mediator.Send(model);
            return CreatedAtAction(nameof(this.Post), result);
        }
    }
}
