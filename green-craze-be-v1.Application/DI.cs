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
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using green_craze_be_v1.Application.Common.Extensions;
using green_craze_be_v1.Application.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace green_craze_be_v1.Application
{
    public static class DI
    {
        [Obsolete]
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapper();
            services.AddProblemDetailsSetup();
            services.AddControllers(options =>
                {
                    options.SuppressAsyncSuffixInActionNames = false;
                })
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

        private static JWTConfigOptions GetJwtConfig(IConfiguration configuration)
        {
            return configuration.GetOptions<JWTConfigOptions>("Tokens");
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = GetJwtConfig(configuration);
            byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtConfig.SigningKey);
            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opts =>
                {
                    opts.RequireHttpsMetadata = false;
                    opts.SaveToken = true;
                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfig.Issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                    };
                    opts.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/app-hub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}