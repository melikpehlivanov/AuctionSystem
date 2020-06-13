namespace Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Application;
    using Application.Common.Models;
    using Application.Users.Commands;
    using Application.Users.Commands.CreateUser;
    using Application.Users.Commands.Jwt.Refresh;
    using Application.Users.Commands.LoginUser;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SwaggerExamples;
    using Swashbuckle.AspNetCore.Annotations;

    public class IdentityController : BaseController
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(nameof(Register))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.IdentityConstants.SuccessfulRegisterRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.IdentityConstants.BadRequestOnRegisterDescriptionMessage,
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Register(CreateUserCommand model)
        {
            await this.Mediator.Send(model);
            return this.Ok();
        }

        /// <summary>
        /// Verifies user credentials and generates JWT and Refresh token
        /// </summary>
        [HttpPost]
        [Route(nameof(Login))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.IdentityConstants.SuccessfulLoginRequestDescriptionMessage,
            typeof(Response<AuthSuccessResponse>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.IdentityConstants.BadRequestOnLoginDescriptionMessage,
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Login(LoginUserCommand model)
        {
            var result = await this.Mediator.Send(model);
            SetRefreshTokenCookie(result.Data.RefreshToken.ToString());
            return this.Ok(result);
        }

        /// <summary>
        /// Verifies the provided token and generates new token and refresh token
        /// </summary>
        [HttpPost]
        [Route(nameof(Refresh))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.IdentityConstants.SuccessfulTokenRefreshRequestDescriptionMessage,
            typeof(Response<AuthSuccessResponse>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.IdentityConstants.BadRequestOnTokenRefreshDescriptionMessage,
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Refresh(JwtRefreshTokenCommand model)
        {
            var refreshToken = this.Request.Cookies["refreshToken"];

            if (refreshToken == null)
            {
                return this.Unauthorized();
            }

            model.RefreshToken = Guid.Parse(refreshToken);
            var result = await this.Mediator.Send(model);
            SetRefreshTokenCookie(result.Data.RefreshToken.ToString());
            return this.Ok(result);
        }

        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMonths(AppConstants.RefreshTokenExpirationTimeInMonths)
            };

            this.Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}