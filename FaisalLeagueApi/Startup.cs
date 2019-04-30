using System;
using System.IO;
using FaisalLeague.Data.Access.DAL;
using FaisalLeagueApi.Filters;
using FaisalLeagueApi.IoC;
using FaisalLeague.Security.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using AutoQueryable;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using AutoQueryable.AspNetCore.Swagger;
using System.Reflection;
using FaisalLeagueApi.SignalR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FaisalLeagueApi.Helpers.Maintenance;
using FaisalLeague.Api.Common;

namespace FaisalLeagueApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            //if (env.IsDevelopment())
            //{
            //    builder.AddApplicationInsightsSettings(developerMode: true);
            //}

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .Build();

            host.Run();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddApplicationInsightsTelemetry(Configuration);

            ContainerSetup.Setup(services, Configuration);



            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (o) =>
            //{
            //    o.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        IssuerSigningKey = TokenAuthOption.Key,
            //        ValidAudience = TokenAuthOption.Audience,
            //        ValidIssuer = TokenAuthOption.Issuer,
            //        ValidateIssuerSigningKey = true,
            //        ValidateLifetime = true,
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ClockSkew = TimeSpan.FromMinutes(0)
            //    };
            //});
            //=============== NEW Support for SignaalR
            services.AddAuthentication(options =>
            {
                // Identity made Cookie authentication the default.
                // However, we want JWT Bearer Auth to be the default.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            // Configure JWT Bearer Auth to expect our security key
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    LifetimeValidator = (before, expires, token, param) =>
                    {
                        return expires > GlobalSettings.CURRENT_DATETIME;
                    },
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateActor = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = TokenAuthOption.Key
                };

            // We have to hook the OnMessageReceived event in order to
            // allow the JWT authentication handler to read the access
            // token from the query string when a WebSocket or 
            // Server-Sent Events request comes in.
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/signalhub")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
            //===============



            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            // Add framework services.
            services.AddMvc(options => { options.Filters.Add(new ApiExceptionFilter()); })
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                });
            ;
            services.AddNodeServices();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Faisal League", Version = "v1" });
                //c.AddAutoQueryable(); // add this line
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                c.AddAutoQueryable(); // add this line
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddElm();

            // for client
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            InitDatabase(app);

            app.UseStaticFiles();

            app.UseAuthentication();

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                //{
                //    HotModuleReplacement = true
                //});

                app.UseSwagger();
            app.UseElmPage();
            app.UseElmCapture();

            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Faisal API V1");
                
                     });

                //app.MapWhen(x => !(x.Request.Path.Value.StartsWith("/swagger") && x.Request.Path.Value.StartsWith("/elm")), builder =>
                //{
                //    builder.UseMvc(routes =>
                //    {
                //        routes.MapSpaFallbackRoute(
                //            "spa-fallback",
                //            new { controller = "Home", action = "Index" });
                //    });
                //});
            //}
            //else
            //{
                //app.UseMvc(routes =>
                //{
                //    routes.MapRoute(
                //        "default",
                //        "{controller=Home}/{action=Index}/{id?}");

                //    routes.MapSpaFallbackRoute(
                //        "spa-fallback",
                //        new { controller = "Home", action = "Index" });
                //});
                //app.UseExceptionHandler("/Home/Error");

            //}

            //for MVC
            app.UseCookiePolicy();
            app.UseWebSockets();
            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalHub>("/signalHub");
            });

            //app.UseMaintenance();

            app.UseMvc();
        }

        private void InitDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MainDbContext>();
                //context.Database.Migrate();
            }
        }
    }
    //public class Startup
    //{
    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    public IConfiguration Configuration { get; }

    //    // This method gets called by the runtime. Use this method to add services to the container.
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    //        // Register the Swagger generator, defining 1 or more Swagger documents
    //        services.AddSwaggerGen(c =>
    //        {
    //            c.SwaggerDoc("v1", new Info { Title = "Faisal API", Version = "v1" });
    //        });
    //    }

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    //    {
    //        // Enable middleware to serve generated Swagger as a JSON endpoint.
    //        app.UseSwagger();

    //        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
    //        // specifying the Swagger JSON endpoint.
    //        app.UseSwaggerUI(c =>
    //        {
    //            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Faisal API V1");
    //            c.RoutePrefix = string.Empty;
    //        });

    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }

    //        app.UseMvc();
    //    }
    //}
}
