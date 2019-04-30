using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Seasons;
using FaisalLeague.Data.Model;

namespace FaisalLeague.Queries
{
    public interface ISeasonsQueryProcessor
    {
        IQueryable<Season> Get();
        Season Get(int id);
        Task<Season> Create(CreateSeasonModel model);
        Task<Season> Update(int id, UpdateSeasonModel model);
        Task Delete(int id);
        Task<Season> ActivateNewSeason();
        Task<Season> GetActiveSeason();
    }
}