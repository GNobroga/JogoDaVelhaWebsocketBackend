using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JogoVelha.Infrastructure;

public static class DbConnectionExtensions 
{
    public static IServiceCollection AddSqliteConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = new SqliteConnection(configuration.GetConnectionString("SqliteConnection"));
        EvolveConfiguration.Configure(connection);
        return services;
    }
}