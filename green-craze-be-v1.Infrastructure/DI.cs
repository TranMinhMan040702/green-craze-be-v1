using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Infrastructure.Data.Context;
using green_craze_be_v1.Infrastructure.Repositories;
using green_craze_be_v1.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure
{
    public static class DI
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddRepositories();
            services.AddDbContextSetup(configuration);
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        public static void AddDbContextSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("AppDBContext")));
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IUploadService, UploadService>()
                .AddScoped<IDateTimeService, DateTimeService>()
                .AddScoped<IUnitService, UnitService>();
        }
    }
}