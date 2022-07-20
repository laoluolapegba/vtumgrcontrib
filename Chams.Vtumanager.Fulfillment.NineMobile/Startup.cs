using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MySql.Core;
using System.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Hangfire.Dashboard;
using System.IO;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Chams.Vtumanager.Provisioning.Infrastructure.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Chams.Vtumanager.Provisioning.Hangfire.Services;
using Chams.Vtumanager.Provisioning.Services.NineMobileEvc;
using Chams.Vtumanager.Provisioning.Services.Mtn;
using Chams.Vtumanager.Provisioning.Services.GloTopup;
using Chams.Vtumanager.Fulfillment.NineMobile.Services;

namespace Sales_Mgmt.Services.Smtp.Hangfire
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            //Configuration = configuration;
            _config = configuration;
            _environment = environment;
        }

        //public IConfiguration Configuration { get; }

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
                
            });

            

            services.AddControllersWithViews();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddHttpClient("PinlessRechargeClient", c =>
            //{
            //    c.BaseAddress = new Uri(_config["EvcSettings:PinlessRecharge:Url"]);
            //});

            services.AddHttpClient("PinlessRechargeClient", c =>
            {
                c.BaseAddress = new Uri(_config["EvcSettings:PinlessRecharge:Url"]);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                return handler;

            });
            services.AddHttpClient("PretupsRechargeClient", c =>
            {
                c.BaseAddress = new Uri(_config["PretupsSettings:Url"]);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                return handler;

            });

            services.AddHttpClient("GloRechargeClient", c =>
            {
                c.BaseAddress = new Uri(_config["GloTopupSettings:Url"]);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                return handler;

            });
            services.AddHttpClient("MtnTopupClient", c =>
            {
                c.BaseAddress = new Uri(_config["MtnTopupSettings:V1:Url"]);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                return handler;

            });
            /*
            var clientCertificate =
                new X509Certificate2(
                  Path.Combine(_environment.ContentRootPath, _config["EvcSettings:Certname"]), _config["EvcSettings:CertPassphrase"]);            

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true,
                SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12
            };
            handler.ClientCertificates.Add(clientCertificate);

            
            services.AddHttpClient("PinlessRechargeClient", c =>
            {
                c.BaseAddress = new Uri(_config["EvcSettings:PinlessRecharge:Url"]);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true,
                    SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12
                };
                handler.ClientCertificates.Add(clientCertificate);
                return handler;

            });
            */



            services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(
                    new MySqlStorage
                    (
                     _config.GetConnectionString("DefaultConnection"),
                     new MySqlStorageOptions
                     {
                         TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                         QueuePollInterval = TimeSpan.FromSeconds(15),
                         JobExpirationCheckInterval = TimeSpan.FromHours(1),
                         CountersAggregateInterval = TimeSpan.FromMinutes(5),
                         PrepareSchemaIfNecessary = true,
                         DashboardJobListLimit = 50000,
                         TransactionTimeout = TimeSpan.FromMinutes(1),
                         //SchemaName = "salesmgmt_dev"
                     }
                    )
                )
                //.UseFilter(new LogFailureAttribute())
            ); 
            services.AddHangfireServer();
            services.AddMvc();

            //configue kestrel
            services.Configure<KestrelServerOptions>(
                        _config.GetSection("Kestrel"));

            services.AddSingleton<IScopeInformation, ScopeInformation>();

            services.AddScoped<IUnitOfWork, UnitOfWork<ChamsProvisioningDbContext>>();
            

            services.AddScoped<ILightEvcService, LightEvcService>();
            services.AddScoped<IMtnTopupService, MtnTopupService>();
            services.AddScoped<IGloTopupService, GloTopupService>();
            services.AddScoped<IAirtelPretupsService, AirtelPretupsService>();


        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseApiExceptionHandler(options =>
            {
                options.AddResponseDetails = UpdateApiErrorResponse;
                options.DetermineLogLevel = DetermineLogLevel;
            });
            app.UseHangfireServer(
                new BackgroundJobServerOptions
                {
                    WorkerCount = 1,
                });

            app.UseHangfireDashboard("/JobDashboard", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });
            RecurringJob.AddOrUpdate<EvcBackgroundTask>(x => x.ProcessPendingRequests(), MinuteInterval(2));
            

            //backgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static string MinuteInterval(int interval)
        {
            return string.Format("*/{0} * * * *", (object)interval);
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

    /// <summary>
    /// 
    /// </summary>
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}
