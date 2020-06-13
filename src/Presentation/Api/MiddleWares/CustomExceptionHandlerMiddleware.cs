namespace Api.Middlewares
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Models.Errors;
    using Newtonsoft.Json;

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
            IDictionary<string, string[]> validationErrorMessages = new Dictionary<string, string[]>();

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    validationErrorMessages = validationException.Failures;
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    errorMsg = badRequestException.Message;
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    errorMsg = notFoundException.Message;
                    break;
                case UnauthorizedException _:
                    code = HttpStatusCode.Unauthorized;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            string result;
            if (validationErrorMessages.Count == 0)
            {
                var errorModel = new ErrorModel
                {
                    Title = code.ToString(),
                    Status = (int) code,
                    TraceId = context.TraceIdentifier,
                    Error = errorMsg
                };
                result = JsonConvert.SerializeObject(errorModel);
            }
            else
            {
                var errorModel = new ValidationErrorModel
                {
                    Title = "One or more validation errors occured.",
                    Status = (int) code,
                    TraceId = context.TraceIdentifier,
                    Errors = validationErrorMessages
                };

                result = JsonConvert.SerializeObject(errorModel);
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}