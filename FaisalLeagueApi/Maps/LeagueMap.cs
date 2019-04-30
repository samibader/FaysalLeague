using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FaisalLeague.Api.Models.Leagues;
using FaisalLeague.Data.Model;

namespace FaisalLeagueApi.Maps
{
    public class LeagueMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<League, LeagueModel>();
            var map3 = configuration.CreateMap<IList<League>, IList<LeagueModel>>();
            map.ForMember(x => x.ParentName, x => x.MapFrom(y => y.ParentId!=null ? y.Parent.Name : "no parent"));
            //map.AfterMap((src, dest) => dest.Children.OrderBy(c=>c.Sort).ToList());
            var map2 = configuration.CreateMap<League, LeagueBasicInfoModel>();
        }
    }
}