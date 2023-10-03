//using Newtonsoft.Json;
using BAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace PresentationLayer.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exeption)
            {
                await HandleExeptionAsync(context, exeption);
            }
        }

        private static Task HandleExeptionAsync(HttpContext context, Exception exception)
        {
            string result;
            switch (exception)
            {
                case NotFoundException:

                    var problemDetails = new ProblemDetails()
                    {
                        Status = 404,
                        Title = exception.Message,
                        Detail = "The requested resource is not found"
                    };

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                    result = JsonSerializer.Serialize(problemDetails);

                    return context.Response.WriteAsync(result);

                default:
                    var code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { error = exception.Message });
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;
                    return context.Response.WriteAsync(result);
            }
        }
    }
}
