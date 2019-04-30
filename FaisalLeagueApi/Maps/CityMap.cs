using System.Linq;
using AutoMapper;
using FaisalLeague.Api.Models.Cities;
using FaisalLeague.Data.Model;

namespace FaisalLeagueApi.Maps
{
    public class CityMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<City, CityMap>();
            //map.ForMember(x => x.Username, x => x.MapFrom(y => y.User.FirstName + " " + y.User.LastName));
        }
    }
}