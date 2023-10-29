using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Domain.Entities;
using green_craze_be_v1.Infrastructure.Data.Context;
using green_craze_be_v1.Infrastructure.Repositories;
using green_craze_be_v1.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<AppDBContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return webApp;
        }

        public static void AddDbContextSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("AppDBContext")));
            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 5;
                opts.Password.RequireDigit = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IUploadService, UploadService>()
                .AddScoped<IDateTimeService, DateTimeService>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<IBrandService, BrandService>()
                .AddScoped<IVariantService, VariantService>()
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICartService, CartService>()
                .AddScoped<IDeliveryService, DeliveryService>()
                .AddScoped<IPaymentMethodService, PaymentMethodService>()
                .AddScoped<IOrderCancellationReasonService, OrderCancellationReasonService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<IProductCategoryService, ProductCategoryService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProductImageService, ProductImageService>()
                .AddScoped<ISaleService, SaleService>()
                .AddScoped<IAddressService, AddressService>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<ITransactionService, TransactionService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IInventoryService, InventoryService>();
        }
    }
}