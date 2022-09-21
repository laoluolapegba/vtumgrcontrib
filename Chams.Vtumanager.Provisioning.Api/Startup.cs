
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
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
//using MySql.Data.MySqlClient;
using Chams.Vtumanager.Provisioning.Api.Helpers.Swagger;
using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Infrastructure;
using Chams.Vtumanager.Provisioning.Infrastructure.Filters;
using Chams.Vtumanager.Provisioning.Infrastructure.Middleware;
using Chams.Vtumanager.Provisioning.Services.QueService;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection;
using MySql.Data.MySqlClient;
using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Chams.Vtumanager.Provisioning.Services.Authentication;
using Chams.Vtumanager.Provisioning.Services.BillPayments;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;
using Chams.Vtumanager.Provisioning.Services.BillPayments.Multichoice;
using Chams.Vtumanager.Provisioning.Services.BillPayments.AbujaDisco;

namespace Chams.Vtumanager.Provisioning.Api
{
    /// <summary>
    /// runtime startup
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
                options.UseMySql(_config.GetConnectionString("DefaultConnection"))
                ;
                //options.UseOracle(_config.GetConnectionString("DefaultConnection"), options => options
                //.UseOracleSQLCompatibility("11"));
                
            });
            services.Configure<KestrelServerOptions>(
            _config.GetSection("Kestrel"));


            services.AddSingleton<IScopeInformation, ScopeInformation>();

            //_settings = _config.GetSection("AmqpServerSettings").Get<AmqpServerSettings>();
            //Uri uri = new Uri("amqp://vtuadmin:Vtu@adm1na@139.59.174.247:5672");
            var rabbitMqSection = _config.GetSection("AmqpServerSettings");
            var exchangeSection = _config.GetSection("AmqpExchange");
            services
            .AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _config.GetSection("AmqpServerSettings:HostName").Value,
                    Port = int.Parse(_config.GetSection("AmqpServerSettings:Port").Value),
                    UserName = _config.GetSection("AmqpServerSettings:UserName").Value,
                    Password = _config.GetSection("AmqpServerSettings:Password").Value,
                    VirtualHost = _config.GetSection("AmqpServerSettings:VirtualHost").Value,
                    AutomaticRecoveryEnabled = true
                };

                return factory.CreateConnection();
            }).AddHealthChecks()
            .Services.AddRabbitMqProducer(rabbitMqSection)
                .AddProductionExchange("amq.direct", exchangeSection);

            //var rabbitMqSection = _config.GetSection("AmqpServerSettings");
            //var exchangeSection = _config.GetSection("AmqpExchange");

            //services.AddRabbitMqServices(rabbitMqSection)
            //    .AddProductionExchange("mtn_exchange", exchangeSection);

            services.Configure<SwaggerSettings>(_config.GetSection(nameof(SwaggerSettings)));

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IAMQService, AMQService>();
            services.AddScoped<ITransactionRecordService, TransactionRecordService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork<ChamsProvisioningDbContext>>();
            services.AddScoped<IMultichoicePaymentsService, MultichoicePaymentsService>();
            services.AddScoped<IBillerPaymentsService, BillerPaymentsService>();
            services.AddHttpClient("BaxiBillsAPI", client =>
            {
                client.BaseAddress = new Uri(_config["BaxiBillsAPI:URL"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            /*
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = _config["JWT:ValidAudience"],
                    ValidIssuer = _config["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]))

                };
            });*/

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
                /*
                .AddSwaggerGen( swagger =>
                {
                    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    });
                    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                              new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}

                        }
                    });
                   
                }
                );
            */
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
                    opts.AddPolicy("ChamsPolicy", builder => builder.WithOrigins(allowOrigins).AllowCredentials());
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(options =>
            {
                options.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    //when authorization has failed, should retrun a json message to client
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = "Unauthorized",
                            Msg = "token expired"
                        }));
                    }
                    //when orther error, retrun a error message json to client
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = "Internal Server Error",
                            Msg = error.Error.Message
                        }));
                    }
                    
                    //when no error, do next.
                    else await next();

                });

                //options.AddResponseDetails = UpdateApiErrorResponse;
                //options.DetermineLogLevel = DetermineLogLevel;

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

            app.UseRouting();
            app.UseAuthentication();
            
            app.UseAuthorization();
            
            //app.UseCors("ChamsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            

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
