using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Leap.API.DB;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class LeapApiDbContextFactory : IDesignTimeDbContextFactory<LeapApiDbContext>
{
    public LeapApiDbContext CreateDbContext(string[] args)
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
        var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddJsonFile("appsettings.local.json", true)
            .Build()
            ;

        var options = new DbContextOptionsBuilder<LeapApiDbContext>();

        ConfigureDbContext(options, configuration);

        return new(options.Options);
    }

    public static void ConfigureDbContext(DbContextOptionsBuilder builder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LeapApi") ?? throw new("Connection string 'LeapApi' is not set");

        builder.UseNpgsql(connectionString);
    }
}