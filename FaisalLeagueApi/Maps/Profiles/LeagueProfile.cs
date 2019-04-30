using AutoMapper;
using FaisalLeague.Api.Models.Leagues;
using FaisalLeague.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Maps.Profiles
{
    public class LeagueProfile : Profile
    {
        public LeagueProfile()
        {
            CreateMap<League, LeagueModel>()
                .ForMember(x => x.ParentName, x => x.MapFrom(y => y.ParentId != null ? y.Parent.Name : "no parent"));

            CreateMap<IList<League>, IList<LeagueModel>>();
            //CreateMap<IQueryable<League>, IQueryable<LeagueModel>>();

            CreateMap<League, LeagueBasicInfoModel>();
            //CreateMap<IQueryable<League>, IQueryable<LeagueBasicInfoModel>>();
        }
    }
}
