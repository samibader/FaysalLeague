using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Data.Access.Maps.Main
{
    public class SeasonMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<Season>()
                .ToTable("Seasons")
                .HasKey(x => x.Id);
        }
    }
}