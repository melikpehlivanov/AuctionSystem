namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Application;
    using Application.Admin.Commands.CreateAdmin;
    using Application.Admin.Commands.DeleteAdmin;
    using Application.Admin.Queries.List;
    using Application.Common.Models;
    using AutoMapper;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using SwaggerExamples;
    using Swashbuckle.AspNetCore.Annotations;

    [Authorize(Roles = AppConstants.AdministratorRole)]
    public class AdminController : BaseController
    {
        private const int CachingTimeInMinutes = 10;

        private readonly IMapper mapper;

        public AdminController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Lists all users with roles
        /// </summary>
        [HttpGet]
        [Cached(CachingTimeInMinutes)]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.AdminConstants.SuccessfulGetRequestDescriptionMessage,
            typeof(PagedResponse<ListAllUsersResponseModel>))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Get([FromQuery] PaginationQuery paginationQuery, [FromQuery] UsersFilter filters)
        {
            var paginationFilter = this.mapper.Map<PaginationFilter>(paginationQuery);
            var model = this.mapper.Map<ListAllUsersQuery>(paginationFilter);
            model.Filters = this.mapper.Map<ListAllUsersQueryFilter>(filters);
            var result = await this.Mediator.Send(model);
            return this.Ok(result);
        }

        /// <summary>
        /// Creates a new administrator
        /// </summary>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            SwaggerDocumentation.AdminConstants.SuccessfulPostRequestDescriptionMessage)]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            SwaggerDocumentation.AdminConstants.BadRequestDescriptionMessage,
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Post([FromBody] CreateAdminCommand model)
        {
            await this.Mediator.Send(model);
            return this.NoContent();
        }

        /// <summary>
        /// Deletes administrator
        /// </summary>
        [HttpDelete]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            SwaggerDocumentation.AdminConstants.SuccessfulDeleteRequestDescriptionMessage)]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            SwaggerDocumentation.AdminConstants.BadRequestDescriptionMessage,
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Delete([FromBody] DeleteAdminCommand model)
        {
            await this.Mediator.Send(model);
            return this.NoContent();
        }
    }
}