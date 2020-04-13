namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Models.Users;
    using Application.Users.Commands.CreateUser;
    using Application.Users.Commands.LoginUser;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models.Errors;
    using Swashbuckle.AspNetCore.Annotations;

    public class IdentityController : BaseController
    {
        private readonly AppSettings appSettings;
        private readonly IMapper mapper;

        public IdentityController(IOptions<AppSettings> appSettings, IMapper mapper)
        {
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
        }

        //TODO: Implement refresh token
     
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(nameof(Register))]
        [SwaggerResponse(StatusCodes.Status200OK, "The user was created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The user data is invalid", typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Register(CreateUserRequestModel model)
        {
            await this.Mediator.Send(this.mapper.Map<CreateUserCommand>(model));
            return this.Ok();
        }

        /// <summary>
        /// Verifies user credentials and generates JWT token
        /// </summary>
        [HttpPost]
        [Route(nameof(Login))]
        [SwaggerResponse(
            StatusCodes.Status200OK, 
            "Jwt token successfully generated", 
            typeof(LoginUserResponseModel))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest, 
            "The user credentials are invalid", 
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Login(LoginUserRequestModel model)
        {
            var appModel = this.mapper.Map<LoginUserCommand>(model);
            appModel.Secret = this.appSettings.Secret;
            var result = await this.Mediator.Send(appModel);
            return this.Ok(result);
        }
    }
}
