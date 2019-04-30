using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.SignalR
{
    public class ActivityResult
    {
        public String ErrorMessage { get; set; }
        public Object Data { get; set; }

        public ActivityResult(string errorMessage = "", Object data = null)
        {
            this.ErrorMessage = errorMessage;
            this.Data = data;
        }
    }
}
