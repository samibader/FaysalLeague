using System.Linq;
using AutoMapper;
using FaisalLeague.Api.Models.Seasons;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;

namespace FaisalLeagueApi.Maps
{
    public class SeasonMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<Season, SeasonModel>();
            //map.ForMember(x => x.Username, x => x.MapFrom(y => y.User.FirstName + " " + y.User.LastName));
        }
    }
}