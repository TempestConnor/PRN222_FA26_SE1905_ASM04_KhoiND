using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HorseClub.Repositories.KhoiND;

public partial class HorseClubDBContext
{
    // The generated context only has the options constructor (OnConfiguring is disabled),
    // so repositories need this one to create their own context.
    public HorseClubDBContext()
    {
    }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            // Per-machine override (e.g. ConnectionStrings__DefaultConnection) without editing appsettings.json.
            .AddEnvironmentVariables()
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{connectionStringName}' is missing from appsettings.json.");
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"))
                          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
