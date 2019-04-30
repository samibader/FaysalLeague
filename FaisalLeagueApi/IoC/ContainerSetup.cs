using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FaisalLeague.Data.Access.DAL;
using FaisalLeagueApi.Filters;
using FaisalLeagueApi.Helpers;
using FaisalLeagueApi.Maps;
using FaisalLeague.Queries;
using FaisalLeagueApi.Security;
using FaisalLeague.Security.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FaisalLeague.Security;
using FaisalLeague.ImageWriter.Classes;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using FaisalLeagueApi.SignalR;
using FaisalLeagueApi.Helpers.Maintenance;
using System.Text;

namespace FaisalLeagueApi.IoC
{
    public static class ContainerSetup
    {
        public static void Setup(IServiceCollection services, IConfigurationRoot configuration)
        {
            AddUow(services, configuration);
            AddQueries(services);
            ConfigureAutoMapper(services);
            ConfigureAuth(services);
            ConfigureImageHandler(services);
            ConfigureSignalR(services);
            //ConfigureMaintenance(services);
        }

        private static void ConfigureSignalR(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
        }

        private static void ConfigureMaintenance(IServiceCollection services)
        {
            MaintenanceMessage message = new MaintenanceMessage()
            {
                Message = "We are sorry, our Service is Unavailable right now !",
                RetryTime = 3200
            };
            services.AddMaintenance(() => false,
             Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(message)), "application/json", message.RetryTime);
        }

        private static void ConfigureAuth(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITokenBuilder, TokenBuilder>();
            services.AddScoped<ISecurityContext, SecurityContext>();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            //var mapperConfig = AutoMapperConfigurator.Configure();
            //var mapper = mapperConfig.CreateMapper();
            //services.AddSingleton(x => mapper);
            //services.AddTransient<IAutoMapper, AutoMapperAdapter>();
            services.AddAutoMapper();
        }

        private static void ConfigureImageHandler(IServiceCollection services)
        {
            services.AddTransient<IImageHandler, ImageHandler>();
            services.AddTransient<IImageWriter,ImageWriter>();
        }

        private static void AddUow(IServiceCollection services, IConfigurationRoot configuration)
        {
            var connectionString = configuration["Data:main"];

            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<MainDbContext>()));

            services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();
            services.AddScoped<UnitOfWorkFilterAttribute>();
        }

        private static void AddQueries(IServiceCollection services)
        {
            var exampleProcessorType = typeof(LeaguesQueryProcessor);
            var types = (from t in exampleProcessorType.GetTypeInfo().Assembly.GetTypes()
                where t.Namespace == exampleProcessorType.Namespace
                      && t.GetTypeInfo().IsClass
                      && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                select t).ToArray();

            foreach (var type in types)
            {
                var interfaceQ = type.GetTypeInfo().GetInterfaces().First();
                services.AddScoped(interfaceQ, type);
            }
        }
    }
}