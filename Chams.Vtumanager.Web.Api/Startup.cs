using AutoMapper;
using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Infrastructure.Filters;
using Chams.Vtumanager.Provisioning.Infrastructure.Middleware;
using Chams.Vtumanager.Provisioning.Infrastructure;
using Chams.Vtumanager.Provisioning.Services.QueService;
using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;
using Chams.Vtumanager.Web.Api.Helpers.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Web.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            //Configuration = configuration;
            _config = configuration;
            _environment = environment;
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(_config);

            services.AddDbContext<ChamsProvisioningDbContext>(options =>
            {
                options.UseMySql(_config.GetConnectionString("DefaultConnection"));
                //options.UseOracle(_config.GetConnectionString("DefaultConnection"), options => options
                //.UseOracleSQLCompatibility("11"));

            });
            services.Configure<KestrelServerOptions>(
            _config.GetSection("Kestrel"));

            services.AddSingleton<IScopeInformation, ScopeInformation>();

            

            services.Configure<SwaggerSettings>(_config.GetSection(nameof(SwaggerSettings)));

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            
            services.AddScoped<ITransactionRecordService, TransactionRecordService>();
            services.AddScoped<IUnitOfWork, UnitOfWork<ChamsProvisioningDbContext>>();

            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    //add global filter for performance tracking
                    options.Filters.Add(typeof(TrackActionPerformanceFilter));

                    // Return a 406 when an unsupported media type was requested
                    options.ReturnHttpNotAcceptable = true;

                    // Add XML formatters
                    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);


            
            services
                .AddApiVersionWithExplorer()
                .AddSwaggerOptions()
                .AddSwaggerGen();
            services.AddResponseCompression();
            // suppress automatic model state validation when using the 
            // ApiController attribute (as it will return a 400 Bad Request
            // instead of the more correct 422 Unprocessable Entity when
            // validation errors are encountered)
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            //Add securty options for CORS cross origin site requests
            var allowOrigins = _config.GetValue<string>("AllowOrigins")?.Split(",") ?? new string[0];
            services
                .AddCors(opts =>
                {
                    opts.AddPolicy("ChamsswitchPolicy", builder => builder.WithOrigins(allowOrigins).AllowCredentials());
                    opts.AddPolicy("PublicEndpoints", builder => builder.SetIsOriginAllowed(IsOriginAllowed));
                    //builder => builder.AllowAnyOrigin()
                    // .WithMethods("Get")
                    // .WithHeaders("Content-Type"));
                });

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        /// <param name="provider">Inject temporary IApiVersionDescriptionProvider</param>
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IApiVersionDescriptionProvider provider
                              //ILoggerFactory loggerFactory
                              )
        {


            app.UseApiExceptionHandler(options =>
            {
                options.AddResponseDetails = UpdateApiErrorResponse;
                options.DetermineLogLevel = DetermineLogLevel;
            });
            app.UseHsts();
            app.UseExceptionHandler("/Error");
            app.UseSwaggerDocuments();
            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/yaml",
                //FileProvider = new PhysicalFileProvider(
                //    Path.Combine(env.WebRootPath, "yaml")),
                //RequestPath = new PathString("/yaml")
            });
            app.UseMvc();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("ChamsswitchPolicy");
            //loggerFactory.AddConsole(_config.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //loggerFactory.AddFile("logs/ts-{Date}.txt");
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseAuthentication();

        }
        /// <summary>
        /// validate allowable origins
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool IsOriginAllowed(string host)
        {
            var corsOriginAllowed = new[] { "localhost" };

            return corsOriginAllowed.Any(origin => host.Contains(origin));
            //return true;
        }
        private void UpdateApiErrorResponse(HttpContext context, Exception ex, ApiError error)
        {
            if (ex.GetType().Name == nameof(MySqlException)) //|| ex.GetType().Name == nameof(OracleException)
            {
                error.Detail = "Exception was a database exception!";
            }
            //error.Links = "https://gethelpformyerror.com/";
        }
        private LogLevel DetermineLogLevel(Exception ex)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
                ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
            {
                return LogLevel.Critical;
            }
            return LogLevel.Error;
        }
    }
}
