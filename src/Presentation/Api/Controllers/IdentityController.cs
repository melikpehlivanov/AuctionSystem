namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Application.Users.Commands.CreateUser;
    using Application.Users.Commands.LoginUser;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class IdentityController : BaseController
    {
        private readonly AppSettings appSettings;

        public IdentityController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        [HttpPost]
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

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login(LoginUserCommand model)
        {
            model.Secret = this.appSettings.Secret;
            var result = await this.Mediator.Send(model);
            return Ok(result);
        }
    }
}
