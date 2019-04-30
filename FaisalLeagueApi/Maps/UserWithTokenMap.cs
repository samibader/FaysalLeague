using System.Linq;
using AutoMapper;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;
using FaisalLeague.Queries.Models;

namespace FaisalLeagueApi.Maps
{
    public class UserWithTokenMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<UserWithToken, UserWithTokenModel>();
        }
    }
}