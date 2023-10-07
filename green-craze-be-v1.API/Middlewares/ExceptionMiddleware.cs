using green_craze_be_v1.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using green_craze_be_v1.Application.Model.CustomAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;
using Hellang.Middleware.ProblemDetails;
using System.Net.Http;

namespace green_craze_be_v1.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ExceptionMiddleware(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory)
        {
            _next = next; _problemDetailsFactory = problemDetailsFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var statusCode = error switch
                {
                    AccessDeniedException => (int)HttpStatusCode.Forbidden,
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                    ValidationException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var problemDetails = _problemDetailsFactory
                    .CreateProblemDetails(context, statusCode: statusCode, detail: error.Message, instance: context.Request.Path);

                string strJson = JsonSerializer.Serialize(problemDetails);
                context.Response.Headers.Add("Content-Type", "application/json");
                await context.Response.WriteAsync(strJson);
            }
        }
    }
}