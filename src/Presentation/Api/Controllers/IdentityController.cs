namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Application.Users.Commands.CreateUser;
    using Microsoft.AspNetCore.Mvc;

    public class IdentityController : BaseController
    {
        [Route(nameof(Register))]
        public async Task<IActionResult> Register(CreateUserCommand model)
        {
            var result = await this.Mediator.Send(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
