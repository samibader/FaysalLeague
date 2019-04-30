using FaisalLeague.Api.Common;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.SignalR
{
    public class SocketUser
    {
        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }

    public class SocketObject
    {
        public object Object { get; set; }
        public string Timestamp { get; set; }
        public string Message { get; set; }
    }

    [Authorize]
    public class SignalHub : Hub
    {
        private readonly ICompetitionQueryProcessor _competitionQueryProcessor;
        private static readonly ConcurrentDictionary<string, SocketUser> SocketUsers
        = new ConcurrentDictionary<string, SocketUser>();

        public SignalHub(ICompetitionQueryProcessor competitionQueryProcessor)
        {
            _competitionQueryProcessor = competitionQueryProcessor;
        }
        public string UserName => Context.User.Identity.Name;

        public async Task SendMessage(string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", message);
            var username = Context.User.Identity.Name;
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }

        public async Task SendMessageToCaller(string message, object obj)
        {
            await Clients.Caller.SendAsync("ReceiveMessagePrivate", JsonConvert.SerializeObject(new SocketObject()
            {
                Timestamp= GlobalSettings.CURRENT_DATETIME_AS_STRING,
                Object=obj,
                Message=message
            }));
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }

        public void Disconnect()
        {
            Context.Abort();
        }

        public async Task NotifyCallerForDuplicateSessions()
        {
            //Clients.Caller.SendAsync("ReceiveMessagePrivate", "Another Session Exists .. Socket will closed " + DateTime.Now.ToString());
            await SendMessageToCaller("Another Session Exists .. Socket will closed", null);
            Disconnect();
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId; 
        }

        public string GetUserIdentifier()
        {
            return Context.UserIdentifier;
        }

        public override async Task OnConnectedAsync()
        {
            var user = SocketUsers.GetOrAdd(UserName, _ => new SocketUser
            {
                Name = UserName,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(Context.ConnectionId);
                if (user.ConnectionIds.Count <=1 )
                {
                    Console.WriteLine($"*********** connection: {Context.ConnectionId} established.");
                }
                else
                    NotifyCallerForDuplicateSessions();
            }
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            SocketUser socketUser;
            SocketUsers.TryGetValue(UserName, out socketUser);

            if (socketUser != null)
            {
                lock (socketUser.ConnectionIds)
                {
                    socketUser.ConnectionIds.RemoveWhere(cid => cid.Equals(Context.ConnectionId));

                    if (!socketUser.ConnectionIds.Any())
                    {
                        SocketUser removedUser;
                        SocketUsers.TryRemove(UserName, out removedUser);
                    }
                }
            }
            Console.WriteLine($"*********** connection: {Context.ConnectionId} disconnected.");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AnswerQuestion(long questionId, long choiceId)
        {
            var x= _competitionQueryProcessor.AnswerQuestion(questionId, choiceId, int.Parse(Context.UserIdentifier));
            await SendMessageToCaller("User Answered from websocket", new { Result = x });
        }

        public async Task GetQuestion(long questionId)
        {
            var x = _competitionQueryProcessor.GetQuestion(questionId);
            await SendMessageToCaller("Question Info",x);
        }

        public async Task BeginTimer()
        {
            var counter = 1;
            var guid = Guid.NewGuid().ToString();
            Task perdiodicTask = PeriodicTaskFactory.Start(() =>
            {
                Clients.Caller.SendAsync("ReceiveMessagePrivate", $"{counter}: {DateTime.Now}");
                counter++;
            }, intervalInMilliseconds: 1000, // fire every one seconds..
              maxIterations: 10, periodicTaskCreationOptions: TaskCreationOptions.RunContinuationsAsynchronously);           // for a total of 20 iterations...

            await perdiodicTask.ContinueWith(_ =>
            {
                Clients.Caller.SendAsync("ReceiveMessagePrivate", $"ENDDDDDD: {DateTime.Now}");
            });
        }
        
    }
}
