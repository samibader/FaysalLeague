using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Common;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.Api.Models.Common;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Model;
using FaisalLeague.Security;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Queries
{
    public class LeaguesQueryProcessor : ILeaguesQueryProcessor
    {
        private readonly IUnitOfWork _uow;
        private readonly ISeasonsQueryProcessor _seasonsQueryProcessor;
        private readonly IUsersQueryProcessor _usersQueryProcessor;

        public LeaguesQueryProcessor(IUnitOfWork uow , ISeasonsQueryProcessor seasonsQueryProcessor, IUsersQueryProcessor usersQueryProcessor)
        {
            _usersQueryProcessor = usersQueryProcessor;
            _seasonsQueryProcessor = seasonsQueryProcessor;
            _uow = uow;
        }

        private IQueryable<League> GetQuery()
        {
            var q = _uow.Query<League>();
            return q;
        }
        public IQueryable<League> GetBaseOnly()
        {
            var query = GetQuery().Where(c=>c.ParentId==null).OrderBy(c=>c.Sort).Include(x => x.Children);
            foreach (var item in query)
            {
                if(item.Children != null) 
                    item.Children = item.Children.OrderBy(c => c.Sort).ToList();
            }
            return query;
        }
        public IQueryable<League> Get()
        {
            var query = GetQuery();
            return query;
        }

        //public IQueryable<League> GetBase()
        //{
        //    var query = GetBaseQuery();
        //    return query;
        //}

        //private IQueryable<League> GetAllQuery()
        //{
        //    var q = _uow.Query<League>().OrderBy(x=>x.Sort).Include(x => x.Children);
        //    //.Where(x => !x.IsDeleted);

        //    //if (!_securityContext.IsAdministrator)
        //    //{
        //    //    var userId = _securityContext.User.Id;
        //    //    q = q.Where(x => x.UserId == userId);
        //    //}

        //    return q;
        //}

        //private IQueryable<League> GetBaseQuery()
        //{
        //    var q = _uow.Query<League>().Where(x => !x.ParentId.HasValue).OrderBy(x => x.Sort).Include(x => x.Children);
        //    //.Where(x => !x.IsDeleted);
        //    foreach (var baseLeague in q)
        //    {
        //        baseLeague.Children= baseLeague.Children.ToList().OrderBy(x => x.Sort);
        //    }
        //    //if (!_securityContext.IsAdministrator)
        //    //{
        //    //    var userId = _securityContext.User.Id;
        //    //    q = q.Where(x => x.UserId == userId);
        //    //}

        //    return q;
        //}

        public League Get(int id)
        {
            var item = GetQuery().Include(c=>c.Parent).Include(c=>c.Children).FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                throw new NotFoundException("League is not found");
            }
            item.Children = item.Children.OrderBy(c => c.Sort).ToList();
            return item;
        }

        
    }
}