using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace BasketService.Api
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

    public static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
    {
      return WebHost.CreateDefaultBuilder()
          .ConfigureAppConfiguration(i => i.AddConfiguration(configuration))
          .UseStartup<Startup>()
          .ConfigureLogging(i => i.ClearProviders())
          //.UseSerilog()
          .Build();
    }

    public static void Main(string[] args)
    {
      var host = BuildWebHost(configuration, args);

      host.Run();
    }

  }
}
