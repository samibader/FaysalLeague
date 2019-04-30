using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.Api.Models.Cities;
using FaisalLeague.Api.Models.Common;
using FaisalLeague.Api.Models.Seasons;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Model;
using FaisalLeague.Security;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Queries
{
    public class CitiesQueryProcessor : ICitiesQueryProcessor
    {
        private readonly IUnitOfWork _uow;

        public CitiesQueryProcessor(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<City> Get()
        {
            var query = GetQuery();
            return query;
        }

        private IQueryable<City> GetQuery()
        {
            var q = _uow.Query<City>().Where(x => !x.IsDeleted); 
            return q;
        }

        public City Get(int id)
        {
            var item = GetQuery().FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                throw new NotFoundException("City is not found");
            }

            return item;
        }

        public async Task<City> Create(CreateCityModel model)
        {
            var item = new City
            {
                Name = model.Name,
                Sort = model.Sort,
            };

            _uow.Add(item);
            await _uow.CommitAsync();

            return item;
        }

        public async Task<City> Update(int id, UpdateCityModel model)
        {
            var item = GetQuery().FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                throw new NotFoundException("City is not found");
            }

            item.Name = model.Name;
            item.Sort = model.Sort;

            await _uow.CommitAsync();
            return item;
        }

        public async Task Delete(int id)
        {
            var item = GetQuery().FirstOrDefault(u => u.Id == id);

            if (item == null)
            {
                throw new NotFoundException("City is not found");
            }

            if (item.IsDeleted) return;

            item.IsDeleted = true;
            await _uow.CommitAsync();
        }
    }
}