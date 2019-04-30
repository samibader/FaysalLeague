using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;
using FaisalLeague.Queries.Models;
using Microsoft.AspNetCore.Http;

namespace FaisalLeague.Queries
{
    public interface ICompetitionQueryProcessor
    {
        Task<ActiveCardModel> GetActiveCardId(int userId);
        QuestionModel GetQuestion(long questionId);
        Task<ActiveCardModel> ActivateUserCard(int userId);
        int AnswerQuestion(long questionId, long choiceId, int userId);
        Task DeclineUserActiveCard(long userCardId);
        Task<UserSeasonLeague> GetUserActiveLeague(int userId);
        Task<UserSeasonLeague> SetUserLeague(int userId, long leagueId);
        Task<IQueryable<User>> GetRankedUsers(long leagueId);

        Task ResetUserPoints(int userId);
        //void ResetAllUsersPoints();
        Task ChangeUserPoints(int userId, int amount);
        Task<int> GetUserPoints(int userId);

        Task<int> GetCurrentUserPoints(int userId);

        //Task<int> GetUserRank(int userId);
    }
}