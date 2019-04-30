using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Seasons;
using FaisalLeague.Data.Model;
using FaisalLeagueApi.Filters;
using FaisalLeagueApi.Maps;
using FaisalLeague.Queries;
using Microsoft.AspNetCore.Mvc;
using AutoQueryable.AspNetCore.Filter.FilterAttributes;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace FaisalLeagueApi.Controllers
{
    [Route("api/[controller]")]
    public class SeasonsController : Controller
    {
        private readonly ISeasonsQueryProcessor _query;
        private readonly IMapper _mapper;

        public SeasonsController(ISeasonsQueryProcessor query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        [HttpGet]
        [AutoQueryable]
        public IQueryable<SeasonModel> Get()
        {
            var result = _query.Get();
            //var models = _mapper.Map<IQueryable<Season>, IQueryable<SeasonModel>>(result);
            var models = result.ProjectTo<SeasonModel>(_mapper.ConfigurationProvider);
            return models;
        }

        [HttpGet("{id}")]
        public SeasonModel Get(int id)
        {
            var item = _query.Get(id);
            var model = _mapper.Map<SeasonModel>(item);
            return model;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<SeasonModel> Post([FromBody]CreateSeasonModel requestModel)
        {
            var item = await _query.Create(requestModel);
            var model = _mapper.Map<SeasonModel>(item);
            return model;
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<SeasonModel> Put(int id, [FromBody]UpdateSeasonModel requestModel)
        {
            var item = await _query.Update(id, requestModel);
            var model = _mapper.Map<SeasonModel>(item);
            return model;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _query.Delete(id);
        }

        [HttpPost("ActivateNewSeason")]
        public async Task<SeasonModel> ActivateNewSeason()
        {
            var season = await _query.ActivateNewSeason();
            var model = _mapper.Map<SeasonModel>(season);
            return model;
        }

        [HttpGet("GetActiveSeason")]
        public async Task<SeasonModel> GetActiveSeason()
        {
            var season = await _query.GetActiveSeason();
            var model = _mapper.Map<SeasonModel>(season);
            return model;
        }
    }
}