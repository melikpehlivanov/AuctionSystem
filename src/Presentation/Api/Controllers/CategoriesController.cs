namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Api.Common;
    using Application.Categories.Queries.List;
    using Application.Common.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SwaggerExamples;
    using Swashbuckle.AspNetCore.Annotations;

    public class CategoriesController : BaseController
    {
        private const int CachedTimeInMinutes = 3600;

        [HttpGet]
        [Cached(CachedTimeInMinutes)]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.CategoriesConstants.SuccessfulGetRequestMessage,
            typeof(MultiResponse<ListCategoriesResponseModel>))]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.CategoriesConstants.BadRequestDescriptionMessage,
            typeof(NotFoundErrorModel))]
        public async Task<IActionResult> Get()
        {
            var result = await this.Mediator.Send(new ListCategoriesQuery());
            return this.Ok(result);
        }
    }
}