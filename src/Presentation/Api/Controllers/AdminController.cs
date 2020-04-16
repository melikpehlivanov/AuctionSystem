namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Api.SwaggerExamples;
    using Application;
    using Application.Admin.Queries.List;
    using Application.Common.Models;
    using Application.Items.Queries.List;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Authorize(AppConstants.AdministratorRole)]
    public class AdminController : BaseController
    {
        private readonly IMapper mapper;

        public AdminController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("getAllUsers")]
        [SwaggerResponse(
            StatusCodes.Status200OK, 
            SwaggerDocumentation.AdminConstants.SuccessfulGetRequestDescriptionMessage,
            typeof(PagedResponse<ListAllUsersResponseModel>))]
        public async Task<IActionResult> GetAllUsers([FromQuery]PaginationQuery paginationQuery, [FromQuery]UsersFilter filters)
        {
            var paginationFilter = this.mapper.Map<PaginationFilter>(paginationQuery);
            var model = this.mapper.Map<ListAllUsersQuery>(paginationFilter);
            model.Filters = this.mapper.Map<ListAllUsersQueryFilter>(filters);
            var result = await this.Mediator.Send(model);
            return Ok(result);
        }
    }
}
