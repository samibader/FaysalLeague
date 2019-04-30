using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Cities;
using FaisalLeague.Data.Model;
using FaisalLeagueApi.Filters;
using FaisalLeagueApi.Maps;
using FaisalLeague.Queries;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoQueryable.AspNetCore.Filter.FilterAttributes;

namespace FaisalLeagueApi.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private readonly ICitiesQueryProcessor _query;
        private readonly IMapper _mapper;

        public CitiesController(ICitiesQueryProcessor query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        [HttpGet]
        [AutoQueryable]
        public IQueryable<CityModel> Get()
        {
            var result = _query.Get();
            var models = result.ProjectTo<CityModel>(_mapper.ConfigurationProvider);
            return models;
        }

        [HttpGet("{id}")]
        public CityModel Get(int id)
        {
            var item = _query.Get(id);
            var model = _mapper.Map<CityModel>(item);
            return model;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<CityModel> Post([FromBody]CreateCityModel requestModel)
        {
            var item = await _query.Create(requestModel);
            var model = _mapper.Map<CityModel>(item);
            return model;
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<CityModel> Put(int id, [FromBody]UpdateCityModel requestModel)
        {
            var item = await _query.Update(id, requestModel);
            var model = _mapper.Map<CityModel>(item);
            return model;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _query.Delete(id);
        }
    }
}