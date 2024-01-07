using JogoVelha.Service.Configuration;
using JogoVelha.Service.Implementation;
using JogoVelha.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JogoVelha.Service;


public static class DependencyInjectionExtensions 
{
    public static IServiceCollection AddServiceLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureTokenService(services, configuration);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }

    private static void ConfigureTokenService(IServiceCollection services, IConfiguration configuration)
    {
        TokenConfiguration tokenConfiguration = new() 
        {
            Issuer = configuration["Jwt:Issuer"]!,
            Secret = configuration["Jwt:Secret"]!,
        };

        services.AddSingleton(tokenConfiguration);
    }
}