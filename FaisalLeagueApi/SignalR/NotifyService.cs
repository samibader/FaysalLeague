using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.SignalR
{
    public class NotifyService
    {
        private readonly IHubContext<SignalHub> _hub;

        public NotifyService(IHubContext<SignalHub> hub)
        {
            _hub = hub;
        }

        public Task SendNotificationAsync(string message)
        {
            return _hub.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
