using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Data.Access.Maps.Main
{
    public class ExpenseMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<Expense>()
                .ToTable("Expenses")
                .HasKey(x => x.Id);
        }
    }
}