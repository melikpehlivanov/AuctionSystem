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
    using Application.Items.Commands;
    using Application.Items.Queries.Details.Models;
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

        /// <summary>
        /// Retrieves all items (max 24 per request)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Returns items",
            typeof(PagedResponse<ListItemsResponseModel>))]
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
            var model = this.mapper.Map<ListItemsQuery>(paginationQuery);
            var result = await this.Mediator.Send(model);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves item with given id
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Successfully found item and returns it.",
            typeof(Response<ItemDetailsResponseModel>))]
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
            var result = await this.Mediator.Send(new GetItemDetailsQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Creates item
        /// </summary>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status201Created, 
            "Creates item successfully and returns the Id of the item",
            typeof(Response<ItemResponseModel>))]
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
