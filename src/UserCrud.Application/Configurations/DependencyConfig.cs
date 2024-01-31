using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.Notifications;
using UserCrud.Application.Services;
using UserCrud.Domain.Models;

namespace UserCrud.Application.Configurations;

public static class DependencyConfig
{
    public static void ResolveDependencies(this IServiceCollection services)
    {
        services
            .AddScoped<INotificator, Notificator>()
            .AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();

        services
            .AddScoped<IAdministratorService, AdministratorService>()
            .AddScoped<IUserService, UserService>();
    }
}