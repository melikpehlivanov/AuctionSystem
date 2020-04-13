namespace Api.Common
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var errorMsg = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    errorMsg = JsonConvert.SerializeObject(validationException.Failures);
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    errorMsg = badRequestException.Message;
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedException _:
                    code = HttpStatusCode.Unauthorized;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var errorModel = new ErrorModel { Title = code.ToString(), Status = (int) code, TraceId = context.TraceIdentifier, Error = errorMsg };
            var result = JsonConvert.SerializeObject(errorModel);
            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
