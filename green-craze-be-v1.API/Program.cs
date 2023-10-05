using green_craze_be_v1.API.Middlewares;
using green_craze_be_v1.Application.Common.Mapper;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Infrastructure.Data.Context;
using green_craze_be_v1.Infrastructure.Repositories;
using green_craze_be_v1.Infrastructure.Services;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddProblemDetails(setup =>
{
    setup.IncludeExceptionDetails = (ctx, env) =>
    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
    || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging";
});
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("AppDBContext")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddScoped<ICurrentUserService, CurrentUserService>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
    .AddScoped<IUnitService, UnitService>();
var app = builder.Build();

app.UseProblemDetails();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();