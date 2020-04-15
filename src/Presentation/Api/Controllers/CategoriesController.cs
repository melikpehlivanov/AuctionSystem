namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Application.Categories.Queries.List;
    using Application.Common.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SwaggerExamples;
    using Swashbuckle.AspNetCore.Annotations;

    public class CategoriesController : BaseController
    {
        [HttpGet]
        [SwaggerResponse(
            StatusCodes.Status200OK, 
            "Returns all categories with their subcategories",
            typeof(Response<ListCategoriesResponseModel>))]
        [SwaggerResponse(
            StatusCodes.Status404NotFound, 
            "Indicates that there are not any categories",
            typeof(NotFoundErrorModel))]
        public async Task<IActionResult> Get()
        {
            var result = await this.Mediator.Send(new ListCategoriesQuery());
            return Ok(result);
        }
    }
}
