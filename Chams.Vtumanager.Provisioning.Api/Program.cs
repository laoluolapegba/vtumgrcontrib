using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Chams.Vtumanager.Provisioning.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Initialize configuration
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        /// <summary>
        /// main entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                //.WriteTo.File(new JsonFormatter(), Configuration["Serilog:LogDir"], shared: true)  //@"c:\temp\logs\Epccos.json"
                .CreateLogger();

            try
            {
                Log.Information("Starting Chamsswitch Provisioning API web host");
                CreateWebHostBuilder(args)
                

                .UseIISIntegration()
                .Build()
                .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Chamsswitch Provisioning API Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .UseContentRoot(Directory.GetCurrentDirectory())
           .UseIISIntegration()
           
           .UseStartup<Startup>()
           // .UseKestrel(options => options.ConfigureEndpoints())
            .UseSerilog();


        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //remvoe default configuration options
            builder.Sources.Clear();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

    }
}
