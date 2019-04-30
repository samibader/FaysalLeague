using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Cities;
using FaisalLeague.Data.Model;

namespace FaisalLeague.Queries
{
    public interface ICitiesQueryProcessor
    {
        IQueryable<City> Get();
        City Get(int id);
        Task<City> Create(CreateCityModel model);
        Task<City> Update(int id, UpdateCityModel model);
        Task Delete(int id);
    }
}