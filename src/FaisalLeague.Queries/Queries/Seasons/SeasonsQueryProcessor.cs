using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Common;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.Api.Models.Common;
using FaisalLeague.Api.Models.Seasons;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Model;
using FaisalLeague.Security;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Queries
{
    public class SeasonsQueryProcessor : ISeasonsQueryProcessor
    {
        private readonly IUnitOfWork _uow;
        private readonly ISecurityContext _securityContext;
        private readonly IUsersQueryProcessor _userQueryProcessor;

        public SeasonsQueryProcessor(IUnitOfWork uow, ISecurityContext securityContext, IUsersQueryProcessor usersQueryProcessor)
        {
            _uow = uow;
            _securityContext = securityContext;
            _userQueryProcessor = usersQueryProcessor;
        }

        public IQueryable<Season> Get()
        {
            var query = GetQuery();
            return query;
        }

        private IQueryable<Season> GetQuery()
        {
            var q = _uow.Query<Season>()
                .Where(x => !x.IsDeleted);

            //if (!_securityContext.IsAdministrator)
            //{
            //    var userId = _securityContext.User.Id;
            //    q = q.Where(x => x.UserId == userId);
            //}

            return q;
        }

        public Season Get(int id)
        {
            var user = GetQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Season is not found");
            }

            return user;
        }

        public async Task<Season> Create(CreateSeasonModel model)
        {
            var item = new Season
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
            };

            _uow.Add(item);
            await _uow.CommitAsync();

            return item;
        }

        public async Task<Season> Update(int id, UpdateSeasonModel model)
        {
            var season = GetQuery().FirstOrDefault(x => x.Id == id);

            if (season == null)
            {
                throw new NotFoundException("Season is not found");
            }

            season.Description = model.Description;
            season.StartDate = model.StartDate;
            season.EndDate = model.EndDate;

            await _uow.CommitAsync();
            return season;
        }

        public async Task Delete(int id)
        {
            var user = GetQuery().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Season is not found");
            }

            if (user.IsDeleted) return;

            user.IsDeleted = true;
            await _uow.CommitAsync();
        }

        public async Task<Season> GetActiveSeason()
        {
            DateTime now = GlobalSettings.CURRENT_DATETIME;
            var result= await Get().Where(x => x.StartDate < now && x.EndDate > now).SingleOrDefaultAsync();
            if (result == null)
                throw new NotFoundException("There is no active season !");
            return result;
        }

        public async Task<Season> ActivateNewSeason()
        {
            try
            {
                var activeSeason = await GetActiveSeason();
                return activeSeason;
            }
            catch (NotFoundException)
            {
                
                    DateTime now = GlobalSettings.CURRENT_DATETIME;
                    //_userQueryProcessor.ResetAllUsersPoints();
                    var item = new Season
                    {
                        StartDate = new DateTime(now.Year, now.Month, 1),
                        EndDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)),
                        Description = string.Format("Season /{0}/ of year {1}", now.Month, now.Year)
                    };

                    _uow.Add(item);
                    await _uow.CommitAsync();

                    return item;
                
            }
        }
    }
}