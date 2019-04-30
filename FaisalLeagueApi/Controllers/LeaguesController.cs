using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Leagues;
using FaisalLeague.Data.Model;
using FaisalLeagueApi.Filters;
using FaisalLeagueApi.Maps;
using FaisalLeague.Queries;
using Microsoft.AspNetCore.Mvc;
using AutoQueryable.AspNetCore.Filter.FilterAttributes;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace FaisalLeagueApi.Controllers
{
    [Route("api/[controller]")]
    public class LeaguesController : Controller
    {
        private readonly ILeaguesQueryProcessor _query;
        private readonly IMapper _mapper;

        public LeaguesController(ILeaguesQueryProcessor query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        [HttpGet("Base")]
        //[QueryableResult]
        [AutoQueryable]
        public IQueryable<LeagueBasicInfoModel> GetBase()
        {
            //var result = _query.GetBaseOnly();
            //var models = _mapper.Map<IQueryable<League>, IQueryable<LeagueBasicInfoModel>>(result);
            //var list = models.ToList();
            //return list;

            var result = _query.GetBaseOnly();
            var models = result.ProjectTo<LeagueBasicInfoModel>(_mapper.ConfigurationProvider);
            return models;
        }

        [HttpGet("Details/Base")]
        //[QueryableResult]
        public IList<LeagueModel> GetBaseDetails()
        {
            var result = _query.GetBaseOnly();
            //var models = _mapper.Map<IQueryable<League>, IQueryable<LeagueModel>>(result);
            var models = result.ProjectTo<LeagueModel>(_mapper.ConfigurationProvider);
            var list = models.ToList();
            foreach (var item in list)
            {
                item.Children = item.Children.OrderBy(c => c.Sort).ToList();
            }

            return list;
        }

        [HttpGet("{id}")]
        public LeagueBasicInfoModel Get(int id)
        {
            var item = _query.Get(id);
            var model = _mapper.Map<LeagueBasicInfoModel>(item);
            return model;
        }

        [HttpGet("Details/{id}")]
        public LeagueModel GetDetails(int id)
        {
            var item = _query.Get(id);
            var model = _mapper.Map<LeagueModel>(item);
            return model;
        }

    }
}