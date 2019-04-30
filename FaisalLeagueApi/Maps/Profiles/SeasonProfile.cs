using AutoMapper;
using FaisalLeague.Api.Common;
using FaisalLeague.Api.Models.Seasons;
using FaisalLeague.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Maps.Profiles
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<Season, SeasonModel>()
                .ForMember(x => x.StartDate, x => x.MapFrom(u => GlobalSettings.ConvertDateTimeToString(u.StartDate)))
                .ForMember(x => x.EndDate, x => x.MapFrom(u => GlobalSettings.ConvertDateTimeToString(u.EndDate))); 
        }
    }
}
