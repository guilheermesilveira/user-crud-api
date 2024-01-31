using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserCrud.Domain.Contracts.Repositories;
using UserCrud.Infra.Data.Context;
using UserCrud.Infra.Data.Repository;

namespace UserCrud.Infra.Data;

public static class DependencyInjection
{
    public static void AddInfraDataConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));

        services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
        {
            dbContextOptions
                .UseMySql(connectionString, serverVersion)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        });

        services.AddScoped<IUserRepository, UserRepository>();
    }
}