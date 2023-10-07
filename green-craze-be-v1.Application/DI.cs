using green_craze_be_v1.Application.Common.Mapper;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.Application
{
    public static class DI
    {
        [Obsolete]
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMapper();
            services.AddProblemDetailsSetup();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var listViolation = new List<green_craze_be_v1.Application.Model.CustomAPI.APIViolation>();
                        context.ModelState.Keys.ToList().ForEach(x =>
                        {
                            listViolation.Add(new green_craze_be_v1.Application.Model.CustomAPI.APIViolation()
                            {
                                Field = x,
                                Messages = context.ModelState[x]?.Errors.Select(e => e.ErrorMessage).ToList()
                            });
                        });
                        return new BadRequestObjectResult(new green_craze_be_v1.Application.Model.CustomAPI.ErrorResponse()
                        {
                            Violations = listViolation,
                            Title = "One or more validation errors occurred",
                            TraceId = context.HttpContext.TraceIdentifier,
                            Instance = context.HttpContext.Request.Path,
                            Status = 400,
                            Type = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                            Code = "VALIDATION_ERROR",
                        });
                    };
                })
                .AddFluentValidation(v =>
                {
                    v.ImplicitlyValidateChildProperties = true;
                    v.ImplicitlyValidateRootCollectionElements = true;
                    v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
            services.AddValidators();
        }

        private static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static void AddProblemDetailsSetup(this IServiceCollection services)
        {
            services.AddProblemDetails(setup =>
            {
                setup.IncludeExceptionDetails = (ctx, env) =>
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging";
            });
        }
    }
}