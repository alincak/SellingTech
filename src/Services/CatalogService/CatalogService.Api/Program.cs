using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Api
{
  public class Program
  {
    private static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    private static IConfiguration configuration
    {
      get
      {
        return new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile($"Configurations/appsettings.json", optional: false)
            .AddJsonFile($"Configurations/appsettings.{env}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
      }
    }

    private static IConfiguration serilogConfiguration
    {
      get
      {
        return new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile($"Configurations/serilog.json", optional: false)
            .AddJsonFile($"Configurations/serilog.{env}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
      }
    }

    public static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
    {
      return WebHost.CreateDefaultBuilder()
          .ConfigureAppConfiguration(i => i.AddConfiguration(configuration))
          .UseWebRoot("Pics")
          .UseContentRoot(Directory.GetCurrentDirectory())
          .UseStartup<Startup>()
          .ConfigureLogging(i => i.ClearProviders())
          .Build();
    }

    public static void Main(string[] args)
    {
      var host = BuildWebHost(configuration, args);

      host.MigrateDbContext<CatalogContext>((context, services) =>
      {
        var env = services.GetService<IWebHostEnvironment>();
        var logger = services.GetService<ILogger<CatalogContextSeed>>();

        new CatalogContextSeed()
            .SeedAsync(context, env, logger)
            .Wait();
      });

      host.Run();
    }
  }
}
