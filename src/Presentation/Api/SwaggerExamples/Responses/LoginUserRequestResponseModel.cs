namespace Api.SwaggerExamples.Responses
{
    using Application.Common.Models;
    using Application.Users.Commands.LoginUser;
    using Swashbuckle.AspNetCore.Filters;

    public class LoginUserRequestResponseModel : IExamplesProvider<Response<LoginUserResponseModel>>
    {
        private const string ExampleToken =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxNDFlZjVlOS0wMmQ2LTQ2MTMtOGFmNS05NTE0NzM3YzI5YTEiLCJ1bmlxdWVfbmFtZSI6InRlc3RAdGVzdC5jb20iLCJuYmYiOjE1ODY3MDQ5ODEsImV4cCI6MTU4NzMwOTc4MSwiaWF0IjoxNTg2NzA0OTgxfQ.GTq2tA4KnCrBkcunnet5ijznq9Vy3NQJq1-znwz0vXI";

        public Response<LoginUserResponseModel> GetExamples()
            => new Response<LoginUserResponseModel>(new LoginUserResponseModel { Token = ExampleToken });
    }
}