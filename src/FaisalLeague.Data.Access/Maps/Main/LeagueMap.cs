using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;


namespace FaisalLeague.Data.Access.Maps.Main
{
    public class LeagueMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<League>()
                .ToTable("Leagues")
                .HasKey(x => x.Id);
            
        }
    }
}