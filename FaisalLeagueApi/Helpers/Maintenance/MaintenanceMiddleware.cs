using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Helpers.Maintenance
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private readonly MaintenanceWindow window;

        public MaintenanceMiddleware(RequestDelegate next, MaintenanceWindow window, ILogger<MaintenanceMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
            this.window = window;
        }

        public MemoryStream GenerateStream(byte[] value)
        {
            return new MemoryStream(value);
        }

        public async Task Invoke(HttpContext context)
        {
            if (window.Enabled)
            {
                // set the code to 503 for SEO reasons
                //MemoryStream ms = GenerateStream(window.Response);
                //context.Response.Body.Write(window.Response);
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.Response.Headers.Add("Retry-After", window.RetryAfterInSeconds.ToString());
                context.Response.ContentType = window.ContentType;

                await context
                    .Response
                    .WriteAsync(Encoding.UTF8.GetString(window.Response), Encoding.UTF8);
            }
            await next.Invoke(context);
        }
    }
}
