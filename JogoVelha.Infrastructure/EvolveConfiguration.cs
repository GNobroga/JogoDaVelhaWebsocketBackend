using System.Data.Common;
using EvolveDb;

namespace JogoVelha.Infrastructure;

public struct EvolveConfiguration 
{
    public static void Configure(DbConnection connection)
    {
       var evolve = new Evolve(connection) {
            Locations = ["/Migrations"],
            IsEraseDisabled = false,
            Command = EvolveDb.Configuration.CommandOptions.Info
        };

        evolve.Migrate();
    }
}