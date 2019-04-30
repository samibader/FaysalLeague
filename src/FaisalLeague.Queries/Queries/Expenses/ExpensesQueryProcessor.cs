using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.Api.Models.Common;
using FaisalLeague.Api.Models.Expenses;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Model;
using FaisalLeague.Security;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Queries
{
    public class ExpensesQueryProcessor : IExpensesQueryProcessor
    {
        private readonly IUnitOfWork _uow;
        private readonly ISecurityContext _securityContext;

        public ExpensesQueryProcessor(IUnitOfWork uow, ISecurityContext securityContext)
        {
            _uow = uow;
            _securityContext = securityContext;
        }

        public IQueryable<Expense> Get()
        {
            var query = GetQuery();
            return query;
        }

        private IQueryable<Expense> GetQuery()
        {
            
            var q = _uow.Query<Expense>()
                .Where(x => !x.IsDeleted);

            if (!_securityContext.IsAdministrator)
            {
                var userId = _securityContext.User.Id;
                q = q.Where(x => x.UserId == userId);
            }

            return q;
        }

        public Expense Get(int id)
        {
            var user = GetQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Expense is not found");
            }

            return user;
        }

        public async Task<Expense> Create(CreateExpenseModel model)
        {
            var item = new Expense
            {
                UserId = _securityContext.User.Id,
                Amount = model.Amount,
                Comment = model.Comment,
                Date = model.Date,
                Description = model.Description,
            };

            _uow.Add(item);
            await _uow.CommitAsync();

            return item;
        }

        public async Task<Expense> Update(int id, UpdateExpenseModel model)
        {
            var expense = GetQuery().FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                throw new NotFoundException("Expense is not found");
            }

            expense.Amount = model.Amount;
            expense.Comment = model.Comment;
            expense.Description = model.Description;
            expense.Date = model.Date;

            await _uow.CommitAsync();
            return expense;
        }

        public async Task Delete(int id)
        {
            var user = GetQuery().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Expense is not found");
            }

            if (user.IsDeleted) return;

            user.IsDeleted = true;
            await _uow.CommitAsync();
        }
    }
}