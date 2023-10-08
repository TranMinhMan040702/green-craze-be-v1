using FluentValidation;
using FluentValidation.AspNetCore;
using green_craze_be_v1.API.Middlewares;
using green_craze_be_v1.Application;
using green_craze_be_v1.Application.Common.Mapper;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Infrastructure;
using green_craze_be_v1.Infrastructure.Data.Context;
using green_craze_be_v1.Infrastructure.Repositories;
using green_craze_be_v1.Infrastructure.Services;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#pragma warning disable CS0612 // Type or member is obsolete
builder.Services.AddApplicationLayer(builder.Configuration);
#pragma warning restore CS0612 // Type or member is obsolete
builder.Services.AddInfrastructureLayer(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = @"JWT authorization header using the Bearer sheme. \r\n\r\n
                        Enter 'Bearer' [space] and then your token in the text input below.
                        \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseProblemDetails();
app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();