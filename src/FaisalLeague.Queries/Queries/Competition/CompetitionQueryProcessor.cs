using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Common;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.Api.Models.Common;
using FaisalLeague.Data.Access.Constants;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Model;
using FaisalLeague.Queries.Models;
using FaisalLeague.Security;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Queries
{
    public class CompetitionQueryProcessor : ICompetitionQueryProcessor
    {
        private readonly IUsersQueryProcessor _usersQueryProcessor;
        private readonly ILeaguesQueryProcessor _leaguesQueryProcessor;
        private readonly ISeasonsQueryProcessor _seasonsQueryProcessor;
        private readonly IUnitOfWork _uow;
        public CompetitionQueryProcessor(IUsersQueryProcessor usersQueryProcessor, IUnitOfWork uow, ILeaguesQueryProcessor leaguesQueryProcessor, ISeasonsQueryProcessor seasonsQueryProcessor)
        {
            _uow = uow;
            _usersQueryProcessor = usersQueryProcessor;
            _leaguesQueryProcessor = leaguesQueryProcessor;
            _seasonsQueryProcessor = seasonsQueryProcessor;
        }

        

        public async Task<ActiveCardModel> GetActiveCardId(int userId)
        {
            var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();
            var model = new ActiveCardModel();
            var userCard = _uow.Query<UserCard>()
                .Include(x => x.Card)
                    .ThenInclude(x => x.CardQuestions)
                    .ThenInclude(x => x.Question)
                    .ThenInclude(x => x.Category)
                .Include(x => x.UserSeasonLeague)
                .Where(x=>x.UserSeasonLeague.SeasonId==activeSeason.Id)
                .Include(x=>x.UserAnswers)
                .Where(x => x.UserSeasonLeague.UserId == userId && x.CardStateId == CardStates.ACTIVE)
                .SingleOrDefault();
            if (userCard == null)
            {
                throw new NotFoundException("no active card for this user !");
            }
            model.CardId = userCard.CardId;
            model.QuestionsByCategoryList = new List<QuestionsByCategory>();
            for (int i = 1; i <= 10; i++)
            {
                QuestionsByCategory q = new QuestionsByCategory();
                q.CategoryId = (from c in _uow.Query<Category>()
                                where c.Sort == i
                                select c
                              ).SingleOrDefault().Id;
                q.CategoryName = (from c in _uow.Query<Category>()
                                    where c.Sort == i
                                    select c
                              ).SingleOrDefault().Name;
                q.QuestionId = userCard.Card.CardQuestions.Select(x => x.Question).Where(c => c.Category.Sort == i).SingleOrDefault().Id;
                if (userCard.UserAnswers.Where(c => c.Question.Category.Sort == i).Any())
                {
                    q.IsAnswered = true;
                }
                else
                    q.IsAnswered = false;
                q.Sort = i;

                model.QuestionsByCategoryList.Add(q);
            }


            model.CardScoreList = new List<CardScore>();
            for (int i = 1; i <= 10; i++)
            {
                CardScore score = new CardScore();
                if (userCard.UserAnswers.Where(c => c.Sort==i).Any())
                {
                    score.Sort = i;
                    score.Score = userCard.UserAnswers.Where(c => c.Sort == i).SingleOrDefault().AnswerPoints;
                }
                else
                {
                    score.Sort = i;
                    score.Score = null;
                }

                model.CardScoreList.Add(score);
            }
            return model;
        }

        public string GetSocketConnection(int userId)
        {
            var q = _uow.Query<UserConnection>()
                    .Where(c => c.UserId == userId)
                    .SingleOrDefault();
            if (q == null)
                return String.Empty;
            return q.ConnectionId;
        }
        public QuestionModel GetQuestion(long questionId)
        {
            var question = _uow.Query<Question>()
                .Include(c => c.Choices)
                .Include(c=>c.Category)
                .Where(c => c.Id == questionId)
                .SingleOrDefault();
            if (question == null)
                throw new NotFoundException("no question for this ID");
            QuestionModel model = new QuestionModel()
            {
                CategoryId = question.CategoryId,
                CategoryName = question.Category.Name,
                DecryptionKey = question.DecryptionKey,
                Id = question.Id,
                Text = question.Text,
                VideoCode = question.VideoCode
            };
            model.Choices = new List<ChoiceModel>();
            foreach (var choice in question.Choices)
            {
                model.Choices.Add(new ChoiceModel()
                {
                    Id = choice.Id,
                    Text = choice.Text,
                    IsCorrect = choice.IsCorrect
                });
            }

            return model;
        }

        public async Task<ActiveCardModel> ActivateUserCard(int userId)
        {
            var userSeasonLeague = await GetUserActiveLeague(userId);
            
            var userCard = _uow.Query<UserCard>()
                .Include(c=>c.UserAnswers)
                    .Where(c => c.UserSeasonLeagueId==userSeasonLeague.Id && c.CardStateId== CardStates.ACTIVE)
                    .SingleOrDefault();
            if (userCard == null || userCard.UserAnswers.Count==10)
            {
                if (userCard != null && userCard.UserAnswers.Count == 10)
                {
                    userCard.CardStateId = CardStates.FINISHED;
                    _uow.Update<UserCard>(userCard);
                    _uow.Commit();
                }

                userCard = new UserCard()
                {
                    CardId=GetRandomCardId(),
                    CardStateId=CardStates.ACTIVE,
                    TimeStamp=GlobalSettings.CURRENT_DATETIME,
                    UserSeasonLeagueId=userSeasonLeague.Id
                };
                _uow.Add<UserCard>(userCard);
                _uow.Commit();
            }
            
            return await GetActiveCardId(userId);
        }

        public long GetRandomCardId()
        {
            var excludedIDs = new HashSet<long>(_uow.Query<UserCard>().Select(p => p.CardId));
            var result = _uow.Query<Card>().Where(p => !excludedIDs.Contains(p.Id));
            //var rand = new Random();
            var cards = result.OrderBy(x => Guid.NewGuid()).ToList();
            if (!cards.Any())
                throw new NotFoundException("There is currently no cards available !");
            return cards[0].Id;
        }

        public int AnswerQuestion(long questionId, long choiceId, int userId)
        {
            var userCard = _uow.Query<UserCard>()
                .Include(x => x.Card)
                .Include(x => x.UserSeasonLeague)
                .Include(x => x.UserAnswers)
                .Where(x => x.UserSeasonLeague.UserId == userId && x.CardStateId == CardStates.ACTIVE)
                .SingleOrDefault();

            if (userCard.UserAnswers.Where(c => c.QuestionId == questionId).Any())
                throw new BadRequestException("Question Already Answered !!");

            var maxSort = userCard.UserAnswers.Any() ? userCard.UserAnswers.Count : 0;
            var newSort = maxSort + 1;
            var answerPoint = _uow.Query<QuestionPoint>()
                .Where(c => c.Id == newSort).SingleOrDefault().Value;
            var correctChoice = _uow.Query<Question>()
                    .Where(c => c.Id == questionId)
                    .Include(c => c.Choices)
                    .SingleOrDefault().Choices.Where(c => c.IsCorrect).SingleOrDefault();

            var userAnswerPoints = 0;

            var userAnswer = new UserAnswer()
            {
                QuestionId = questionId,
                Sort = newSort,
                TimeStamp = DateTime.Now,
                UserCardId = userCard.Id
            };
            if(choiceId==0)
            {
                userAnswer.ChoiceId = null;
                userAnswerPoints = 0;
            }
            else
            {
                userAnswer.ChoiceId = choiceId;
                userAnswerPoints = choiceId == correctChoice.Id ? answerPoint : -answerPoint;
            }

            userAnswer.AnswerPoints = userAnswerPoints;
            _uow.Add<UserAnswer>(userAnswer);
            _uow.Commit();

            //if(newSort==10)
            //{
            //    userCard.CardStateId = CardStates.FINISHED;
            //    _uow.Update<UserCard>(userCard);
            //    _uow.Commit();
            //}

            //await ChangeUserPoints(userId, userAnswerPoints);
            userCard.UserSeasonLeague.Points += userAnswerPoints;
            _uow.Update<UserCard>(userCard);
            _uow.Commit();

            return userAnswerPoints;

            //if (choiceId == 0)
            //    return 0;
            //else if (choiceId == correctChoice.Id)
            //    return answerPoint;
            //else
            //    return -answerPoint;
        }

        public async Task DeclineUserActiveCard(long userCardId)
        {
            var userCard = _uow.Query<UserCard>()
                .Where(c => c.Id == userCardId)
                .Include(c=>c.UserSeasonLeague)
                .Include(c=>c.Card)
                    .ThenInclude(c=>c.CardQuestions)
                .Include(c=>c.UserAnswers)
                .SingleOrDefault();
            
            var userId = userCard.UserSeasonLeague.UserId;

            // Loop to add remaining answers as 0
            var cardQuestions = userCard.Card.CardQuestions;
            foreach (var question in cardQuestions)
            {
                if(!userCard.UserAnswers.Where(c=>c.QuestionId==question.QuestionId).Any())
                {
                    AnswerQuestion(question.QuestionId, 0, userId);
                }
            }

            // Decline Card Status
            userCard.CardStateId = CardStates.DECLINED;

        }

        public async Task<UserSeasonLeague> GetUserActiveLeague(int userId)
        {
            var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();

            var activeUserLeague = await _uow.Query<UserSeasonLeague>().Where(c => c.IsActive &&
                 c.SeasonId == activeSeason.Id &&
                 c.UserId == userId
            ).SingleOrDefaultAsync();

            if (activeUserLeague == null)
                throw new NotFoundException("User don't have league for this season !");
            return activeUserLeague;

        }

        public async Task<UserSeasonLeague> SetUserLeague(int userId, long leagueId)
        {
            var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();

            var activeUserLeague = await _uow.Query<UserSeasonLeague>().Where(c => c.IsActive &&
                 c.SeasonId == activeSeason.Id &&
                 c.UserId == userId
            ).SingleOrDefaultAsync();

            if (activeUserLeague != null)
            {
                // check user already chosen this league
                if (activeUserLeague.LeagueId == leagueId)
                    throw new BadRequestException("This user already in this league!");
                
                // decline all user cards in this league (active & finished)
                var userCards = _uow.Query<UserCard>()
                    .Where(c => c.UserSeasonLeagueId == activeUserLeague.Id);
                if(userCards!=null || userCards.Any())
                {
                    foreach (var card in userCards)
                    {
                        await DeclineUserActiveCard(card.Id);
                    }
                }
                    
                // Reset User Points
                //await ResetUserPoints(userId);
                // Set UserSeasonLeague as InActive
                activeUserLeague.IsActive = false;
                _uow.Update<UserSeasonLeague>(activeUserLeague);
                _uow.Commit();
            }

            UserSeasonLeague newActiveUserLeague = new UserSeasonLeague()
            {
                IsActive = true,
                LeagueId = leagueId,
                SeasonId = activeSeason.Id,
                SubscriptionDateTime = GlobalSettings.CURRENT_DATETIME,
                UserId = userId
            };
            _uow.Add<UserSeasonLeague>(newActiveUserLeague);
            _uow.Commit();

            return newActiveUserLeague;
        }

        public async Task<IQueryable<User>> GetRankedUsers(long leagueId)
        {
            var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();
            var query = _uow.Query<UserSeasonLeague>()
                    .Where(c => c.SeasonId == activeSeason.Id && c.LeagueId == leagueId && c.IsActive)
                    .Include(c => c.User)
                    .Select(c=>c.User)
                    .Where(c=>!c.IsDeleted)
                    //.OrderByDescending(c=>c.Points)
                    ;
            return query;
        }

        public async Task ResetUserPoints(int userId)
        {
            var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();

            var activeUserLeague = await _uow.Query<UserSeasonLeague>().Where(c => c.IsActive &&
                 c.SeasonId == activeSeason.Id &&
                 c.UserId == userId
            ).SingleOrDefaultAsync();
            activeUserLeague.Points = 0;
            _uow.Update<UserSeasonLeague>(activeUserLeague);
            _uow.Commit();
        }

        public async Task ChangeUserPoints(int userId, int amount)
        {
            var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();

            var activeUserLeague = await _uow.Query<UserSeasonLeague>().Where(c => c.IsActive &&
                 c.SeasonId == activeSeason.Id &&
                 c.UserId == userId
            ).SingleOrDefaultAsync();
            activeUserLeague.Points += amount;
            _uow.Update<UserSeasonLeague>(activeUserLeague);
            _uow.Commit();
        }

        public async Task<int> GetUserPoints(int userId)
        {
            var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();

            var activeUserLeague = await _uow.Query<UserSeasonLeague>().Where(c => c.IsActive &&
                 c.SeasonId == activeSeason.Id &&
                 c.UserId == userId
            ).SingleOrDefaultAsync();

            return activeUserLeague.Points;
        }

        public async Task<int> GetCurrentUserPoints(int userId)
        {
            var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();

            var activeUserLeague = await _uow.Query<UserSeasonLeague>().Where(c => c.IsActive &&
                 c.SeasonId == activeSeason.Id &&
                 c.UserId == userId
            ).SingleOrDefaultAsync();

            if (activeUserLeague == null)
                return int.MinValue;
            return activeUserLeague.Points;
        }

        //public async Task<int> GetUserRank(int userId)
        //{
        //    var activeSeason = await _seasonsQueryProcessor.GetActiveSeason();

        //    var activeUserLeague = await _uow.Query<UserSeasonLeague>().Where(c => c.IsActive &&
        //         c.SeasonId == activeSeason.Id &&
        //         c.UserId == userId
        //    ).SingleOrDefaultAsync();

            
        //}

        //public void ResetAllUsersPoints()
        //{
        //    var users = Get();
        //    foreach (var user in users)
        //    {
        //        ResetUserPoints(user.Id);
        //    }
        //}
    }
}
