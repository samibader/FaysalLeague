using AutoMapper;
using FaisalLeague.Api.Models.Cities;
using FaisalLeague.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Maps.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityModel>();
        }
    }
}
