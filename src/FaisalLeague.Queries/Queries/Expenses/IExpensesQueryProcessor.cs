using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Expenses;
using FaisalLeague.Data.Model;

namespace FaisalLeague.Queries
{
    public interface IExpensesQueryProcessor
    {
        IQueryable<Expense> Get();
        Expense Get(int id);
        Task<Expense> Create(CreateExpenseModel model);
        Task<Expense> Update(int id, UpdateExpenseModel model);
        Task Delete(int id);
    }
}