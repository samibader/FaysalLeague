using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoQueryable.AspNetCore.Filter.FilterAttributes;
using FaisalLeague.Api.Common;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;
using FaisalLeague.Queries;
using FaisalLeague.Queries.Models;
using FaisalLeague.Security;
using FaisalLeagueApi.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace FaisalLeagueApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly ICompetitionQueryProcessor _competitionQueryProcessor;
        private readonly IUsersQueryProcessor _usersQueryProcessor;
        private readonly ISecurityContext _securityContext;
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly IMapper _mapper;
        public CompetitionController(ICompetitionQueryProcessor competitionQueryProcessor, ISecurityContext securityContext, IHubContext<SignalHub> hubContext, IMapper mapper, IUsersQueryProcessor usersQueryProcessor)
        {
            _competitionQueryProcessor = competitionQueryProcessor;
            _securityContext = securityContext;
            _hubContext = hubContext;
            _mapper = mapper;
            _usersQueryProcessor = usersQueryProcessor;
        }

        [Authorize]
        [HttpGet("GetActiveCard")]
        public async Task<ActiveCardModel> GetActiveCard()
        {
            var userId = _securityContext.User.Id;
            return await _competitionQueryProcessor.GetActiveCardId(userId);
        }

        [Authorize]
        [HttpPost("ActivateUserCard")]
        public async Task<ActiveCardModel> ActivateUserCard()
        {
            var userId = _securityContext.User.Id;
            var model = await _competitionQueryProcessor.ActivateUserCard(userId);
            return model;
        }

        //[Authorize]
        //[HttpPost("AnswerQuestion/{questionId}/{choiceId}")]
        //public async Task<int> AnswerQuestion(long questionId, long choiceId)
        //{
        //    var userId = _securityContext.User.Id;
        //    return await _competitionQueryProcessor.AnswerQuestion(questionId, choiceId, userId);
        //}

        //[Authorize]
        //[HttpGet("GetQuestion/{questionid}")]
        //public QuestionModel GetQuestion(long questionid)
        //{
        //    return _competitionQueryProcessor.GetQuestion(questionid);
        //}

        
        [HttpGet("AnswerQuestionTimer/{questionid}")]
        [Authorize]
        public async Task<bool> AnswerQuestionTimer(long questionid)
        {
            var userId = _securityContext.User.Id;
            var counter = 1;
            Task perdiodicTask = PeriodicTaskFactory.Start(() =>
            {
                Console.WriteLine($"===========================Begin({counter})=========================");
                //_hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessagePrivate", $"{counter}: {DateTime.Now}");
                _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessagePrivate", JsonConvert.SerializeObject(new SocketObject()
                {
                    Timestamp = GlobalSettings.CURRENT_DATETIME_AS_STRING,
                    Object = new {Counter=counter },
                    Message = "Tick"
                }));
                counter++;
            }, intervalInMilliseconds: 1000, // fire every one seconds..
              maxIterations: 20, periodicTaskCreationOptions: TaskCreationOptions.RunContinuationsAsynchronously);           // for a total of 20 iterations...

            await perdiodicTask.ContinueWith(_ =>
            {
                Console.WriteLine("===========================End=========================");
                _competitionQueryProcessor.AnswerQuestion(questionid, 0, userId);
                //_hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessagePrivate", $"User did't answer, result is 0 {DateTime.Now}");
                _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessagePrivate", JsonConvert.SerializeObject(new SocketObject()
                {
                    Timestamp = GlobalSettings.CURRENT_DATETIME_AS_STRING,
                    Object = new { Result = 0 },
                    Message = "User did't answer"
                }));
            });


            return true;
        }


        [Authorize]
        [HttpPost("GetActiveLeague")]
        public async Task<UserActiveLeague> GetActiveLeague()
        {
            var userId = _securityContext.User.Id;
            var item = await _competitionQueryProcessor.GetUserActiveLeague(userId);
            if (item == null)
                throw new NotFoundException("User don't have league for this season !");
            var model = _mapper.Map<UserActiveLeague>(item);
            return model;
        }

        [Authorize]
        [HttpPost("SetLeague/{leagueId}")]
        public async Task<UserActiveLeague> SetLeague(long leagueId)
        {
            var userId = _securityContext.User.Id;
            var item = await _competitionQueryProcessor.SetUserLeague(userId, leagueId);

            var model = _mapper.Map<UserActiveLeague>(item);
            return model;
        }

        [HttpGet("GetRankedUsersByLeague/{leagueId}")]
        [AutoQueryable]
        public async Task<IQueryable<QueryableUserModel>> GetRankedUsersByLeague(long leagueId)
        {
            var result = await _competitionQueryProcessor.GetRankedUsers(leagueId);
            var models = result.ProjectTo<QueryableUserModel>(_mapper.ConfigurationProvider);
            return models;
        }

        [HttpGet("GetCurrentUserRank")]
        [Authorize]
        public async Task<int> GetCurrentUserRank()
        {
            var userId = _securityContext.User.Id;
            var userLeague = await _competitionQueryProcessor.GetUserActiveLeague(userId);

            var user = _usersQueryProcessor.Get(userId);
            var userModel = _mapper.Map<QueryableUserModel>(user);


            var result = await _competitionQueryProcessor.GetRankedUsers(userLeague.LeagueId);
            var models = result.ProjectTo<QueryableUserModel>(_mapper.ConfigurationProvider).OrderByDescending(c=>c.Points).Select(c=>c.Id).ToList();
            return models.IndexOf(userId)+1;
        }
    }
}