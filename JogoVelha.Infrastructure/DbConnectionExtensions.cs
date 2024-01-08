using JogoVelha.Infrastructure.Repositories;
using JogoVelha.Infrastructure.Repositories.Implementation;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace JogoVelha.Infrastructure;

public static class DbConnectionExtensions 
{
    public static IServiceCollection AddSqliteConfiguration(this IServiceCollection services, IConfiguration configuration)
    {   
        var connection = new SqliteConnection(configuration.GetConnectionString("SqliteConnection"));
        EvolveConfiguration.Configure(connection);
        return services;
    }

    public static IServiceCollection AddPostgresConfiguration(this IServiceCollection services, IConfiguration configuration)
    {   
        var connection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSQLConnection"));
        EvolveConfiguration.Configure(connection);
        return services;
    }


    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}