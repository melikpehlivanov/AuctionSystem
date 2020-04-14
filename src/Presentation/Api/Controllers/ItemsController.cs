namespace Api.Controllers
{
    using System.Threading.Tasks;
    using SwaggerExamples;
    using Application.Items.Commands.CreateItem;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using Application.Items.Queries.Details;
    using Application.Common.Models;
    using Application.Items.Queries.List;
    using AutoMapper;

    [Authorize]
    public class ItemsController : BaseController
    {
        private readonly IMapper mapper;

        public ItemsController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Successfully found item and returns it.",
            typeof(ItemDetailsResponseModel))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Item with the provided id does not exist",
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            "Available only for authorized users",
            typeof(UnauthorizedErrorModel))]
        public async Task<IActionResult> Get([FromQuery]PaginationQuery paginationQuery)
        {
            var model = this.mapper.Map<ListItemsRequest>(paginationQuery);
            var result = await this.Mediator.Send(model);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Successfully found item and returns it.",
            typeof(ItemDetailsResponseModel))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Item with the provided id does not exist",
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            "Available only for authorized users",
            typeof(UnauthorizedErrorModel))]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await this.Mediator.Send(new GetItemDetailsRequest(id));
            return Ok(result);
        }

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
