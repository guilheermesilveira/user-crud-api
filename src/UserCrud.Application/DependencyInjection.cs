using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UserCrud.Application.Configurations;

namespace UserCrud.Application;

public static class DependencyInjection
{
    public static void AddApplicationConfig(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddAuthConfig(builder);
        
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        
        services.AddCorsConfig();
        
        services.ResolveDependencies();
    }
}