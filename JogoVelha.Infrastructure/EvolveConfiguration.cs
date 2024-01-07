using System.Data.Common;
using EvolveDb;
using Microsoft.Extensions.Logging;

namespace JogoVelha.Infrastructure;

public struct EvolveConfiguration 
{   
    public static void Configure(DbConnection connection)
    {
        try 
        {
            var evolve = new Evolve(connection) 
            {
                Locations = [Path.Combine("..", "JogoVelha.Infrastructure", "Database", "Migrations")],
                IsEraseDisabled = false,
            };

            evolve.Migrate();
        }
        catch 
        {
            Console.WriteLine("Database migration failed");
        }
    }
}