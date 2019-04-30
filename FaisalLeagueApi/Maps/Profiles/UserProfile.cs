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
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>()
            .ForMember(x => x.DOB, x => x.MapFrom(u => GlobalSettings.ConvertDateToString(u.DOB)))
            .ForMember(x => x.CityName, x => x.MapFrom(u => u.City.Name))
            //.ForMember(x => x.Points, u => u.ResolveUsing<PointsResolver>()); 
            .ForMember(x => x.Points, u => u.MapFrom(c => c.UserSeasonLeagues.Where(z=>z.IsActive).Any() ? c.UserSeasonLeagues.Where(z => z.IsActive).OrderByDescending(r=>r.Id).First().Points : (int?)null));
            //.ForMember(x => x.ActiveLeague, u => u.ResolveUsing<ActiveLeagueResolver>());

            CreateMap<User, QueryableUserModel>()
                .ForMember(x => x.Points, u => u.MapFrom(c => c.UserSeasonLeagues.Where(z => z.IsActive).Any() ? c.UserSeasonLeagues.Where(z => z.IsActive).OrderByDescending(r => r.Id).First().Points : (int?)null));

            CreateMap<UserWithToken, UserWithTokenModel>()
                 .ForMember(x => x.ExpiresAt, x => x.MapFrom(u => GlobalSettings.ConvertDateTimeToString(u.ExpiresAt)));
        }
    }

    public class ActiveLeagueResolver : IValueResolver<User, object, long>
    {
        private readonly ICompetitionQueryProcessor _competitionQueryProcessor;

        public ActiveLeagueResolver(ICompetitionQueryProcessor competitionQueryProcessor)
        {
            _competitionQueryProcessor = competitionQueryProcessor;
        }


        long IValueResolver<User, object, long>.Resolve(User source, object destination, long destMember, ResolutionContext context)
        {
            var x = Task.Run(async () => await _competitionQueryProcessor.GetUserActiveLeague(source.Id))
                       .Result;
            if (x == null)
                return 0;
            return x.Id;

        }
    }

    public class PointsResolver : IValueResolver<User, object, int>
    {
        private readonly ICompetitionQueryProcessor _competitionQueryProcessor;

        public PointsResolver(ICompetitionQueryProcessor competitionQueryProcessor)
        {
            _competitionQueryProcessor = competitionQueryProcessor;
        }


        int IValueResolver<User, object, int>.Resolve(User source, object destination, int destMember, ResolutionContext context)
        {
            var x = Task.Run(async () => await _competitionQueryProcessor.GetCurrentUserPoints(source.Id))
                       .Result;
            //if (x == null)
            //    return 0;
            return x;

        }
    }
}
