using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;
using FaisalLeague.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace FaisalLeagueApi.Maps
{
    public class UserMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<User, UserModel>();
            //map.ForMember(x => x.Roles, x => x.MapFrom(u => u.Roles.Select(r => r.Role.Name).ToArray()));
            map.ForMember(x => x.DOB, x => x.MapFrom(u => u.DOB.ToString("dd/MM/yyyy")));
            map.ForMember(x => x.CityName, x => x.MapFrom(u => u.City.Name));
            map.ForMember(x => x.ActiveLeague, u => u.ResolveUsing<ActiveLeagueResolver>());
            //configuration.ConstructServicesUsing(type => typeof(ILeaguesQueryProcessor));
            configuration.CreateMap<User, QueryableUserModel>();
        }
    }

    public class ActiveLeagueResolver : IValueResolver<User, object, long>
    {
        private readonly ILeaguesQueryProcessor _leaguesQueryProcessor;

        public ActiveLeagueResolver(ILeaguesQueryProcessor leaguesQueryProcessor)
        {
            _leaguesQueryProcessor = leaguesQueryProcessor;
        }
        

        long IValueResolver<User, object, long>.Resolve(User source, object destination, long destMember, ResolutionContext context)
        {

            return Task.Run(async () => await _leaguesQueryProcessor.GetUserActiveLeague(source.Id))
                    .Result
                    .Id;
        }
    }
}