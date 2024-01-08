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
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}