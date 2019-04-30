using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Model;
using FaisalLeague.Security;
using FaisalLeagueApi.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FaisalLeagueApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestSignalRController : ControllerBase
    {
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly ISecurityContext _context;
        private readonly IUnitOfWork _uow;
        public TestSignalRController(IHubContext<SignalHub> hubContext, ISecurityContext context, IUnitOfWork uow)
        {
            _hubContext = hubContext;
            _context = context;
            _uow = uow;
            
        }

        [HttpGet]
        [Authorize]
        public async Task<bool> GetCurrentUser()
        {
            var userId = _context.User.Id.ToString();
            var counter = 1;
             Task perdiodicTask =  PeriodicTaskFactory.Start(() =>
            {
                Console.WriteLine($"===========================Begin({counter})=========================");
                _hubContext.Clients.User(userId).SendAsync("ReceiveMessagePrivate", $"{counter}: {DateTime.Now}");
                counter++;
            }, intervalInMilliseconds: 1000, // fire every one seconds..
               maxIterations: 10,periodicTaskCreationOptions: TaskCreationOptions.RunContinuationsAsynchronously);           // for a total of 20 iterations...

            await perdiodicTask.ContinueWith(_ =>
            {
                Console.WriteLine("===========================End=========================");
                _hubContext.Clients.User(userId).SendAsync("ReceiveMessagePrivate", $"ENDDDDDDDDD: {DateTime.Now}");
            });


            return true;
        }

        [HttpGet("GetConnectionId")]
        [Authorize]
        public async Task<string> GetConnectionId()
        {
            var userId = _context.User.Id;
            var connection = (from u in _uow.Query<UserConnection>()
                        where u.UserId==userId
                        select u).FirstOrDefault();
            if (connection != null)
                return connection.ConnectionId;
            return "";
        }
    }
}