using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JogoVelha.Infrastructure;

public static class DbConnectionExtensions 
{
    public static IServiceCollection AddSqliteConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var connection = new SqliteConnection(config.GetConnectionString("SqliteConnection"));
        EvolveConfiguration.Configure(connection);
        return services;
    }
}