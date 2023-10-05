using green_craze_be_v1.Application.Intefaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace green_craze_be_v1.Infrastructure.Data.Context
{
    public class AppDBContextFactory : IDesignTimeDbContextFactory<AppDBContext>
    {
        private readonly ICurrentUserService _currentUserService;

        public AppDBContextFactory()
        {
        }

        public AppDBContextFactory(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public AppDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("AppDBContext");

            var optionBuilder = new DbContextOptionsBuilder<AppDBContext>();
            optionBuilder.UseMySQL(connectionString);

            return new AppDBContext(optionBuilder.Options, _currentUserService);
        }
    }
}