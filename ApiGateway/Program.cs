using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.AddServerHeader = false;
                    });

                    webBuilder.UseStartup<Startup>();

                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        //var _env = $"ocelot.{env}.json";
                        config.AddJsonFile($"ocelot.{env}.json", true, true);
                        //config.AddJsonFile(_env);
                        // config.AddJsonFile("ocelot.ContainerDev.json");
                    });
                });
    }
}
