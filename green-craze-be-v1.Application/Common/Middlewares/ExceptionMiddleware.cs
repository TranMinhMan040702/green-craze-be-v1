using green_craze_be_v1.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using green_craze_be_v1.Application.Model.CustomAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;

namespace green_craze_be_v1.Application.Common.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        { _next = next; }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = new ErrorResponse
                {
                    Status = error switch
                    {
                        AccessDeniedException => (int)HttpStatusCode.Forbidden,
                        NotFoundException => (int)HttpStatusCode.NotFound,
                        UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                        ValidationException => (int)HttpStatusCode.BadRequest,
                        _ => (int)HttpStatusCode.InternalServerError,
                    },

                    Title = error.Message,
                    Type = new Uri(error.HelpLink ?? "about:blank"),
                    TraceId = context.TraceIdentifier,
                    Detail = error.InnerException.Message,
                    Instance = new Uri(context.Request.GetDisplayUrl())
                };
                string strJson = JsonSerializer.Serialize(response);
                context.Response.Headers.Add("Content-Type", "application/json");
                await context.Response.WriteAsync(strJson);
            }
        }
    }
}