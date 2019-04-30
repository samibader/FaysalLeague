using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Seasons;
using FaisalLeague.Data.Model;

namespace FaisalLeague.Queries
{
    public interface ILeaguesQueryProcessor
    {
        IQueryable<League> Get();
        IQueryable<League> GetBaseOnly();
        League Get(int id);
    }
}