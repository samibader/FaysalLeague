using AutoMapper;
using FaisalLeague.Api.Common;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;
using FaisalLeague.Queries;
using FaisalLeague.Queries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Maps.Profiles
{
    public class UserSeasonLeagueProfile : Profile
    {
        public UserSeasonLeagueProfile()
        {
            CreateMap<UserSeasonLeague, UserActiveLeague>()
                .ForMember(x => x.SubscriptionDateTime, x => x.MapFrom(u => GlobalSettings.ConvertDateTimeToString(u.SubscriptionDateTime)));
        }
    }
}
