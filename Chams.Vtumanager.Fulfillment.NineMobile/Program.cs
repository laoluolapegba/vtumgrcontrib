using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;

namespace Sales_Mgmt.Services.Smtp.Hangfire
{
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)

                .CreateLogger();

            try
            {
                Log.Information("Starting Hangfire host");
                CreateHostBuilder(args)
                
                
                .Build()
                .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Hangfire Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }


        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //remvoe default configuration options
            builder.Sources.Clear();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        //public static IHostBuilder CreatHostBuilder(string[] args) =>
        //   WebHost.CreateDefaultBuilder(args)
        //   .UseStartup<Startup>()
        //    .UseSerilog();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseSerilog();
                });
    }
}
