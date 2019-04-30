using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Helpers.Maintenance
{
    public static class MaintenanceWindowExtensions
    {
        public static IServiceCollection AddMaintenance(this IServiceCollection services, MaintenanceWindow window)
        {
            services.AddSingleton(window);
            return services;
        }

        public static IServiceCollection AddMaintenance(this IServiceCollection services, Func<bool> enabler, byte[] response, string contentType = "text/html", int retryAfterInSeconds = 3600)
        {
            AddMaintenance(services, new MaintenanceWindow(enabler, response)
            {
                ContentType = contentType,
                RetryAfterInSeconds = retryAfterInSeconds
            });

            return services;
        }

        public static IApplicationBuilder UseMaintenance(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MaintenanceMiddleware>();
        }
    }
}
