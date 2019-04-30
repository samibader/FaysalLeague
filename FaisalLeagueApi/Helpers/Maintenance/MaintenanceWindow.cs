using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Helpers.Maintenance
{
    public class MaintenanceWindow
    {

        private Func<bool> enabledFunc;
        private byte[] response;

        public MaintenanceWindow(Func<bool> enabledFunc, byte[] response)
        {
            this.enabledFunc = enabledFunc;
            this.response = response;
        }

        public bool Enabled => enabledFunc();
        public byte[] Response => response;

        public int RetryAfterInSeconds { get; set; } = 3600;
        public string ContentType { get; set; } = "application/json";
    }
}
