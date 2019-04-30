using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;


namespace FaisalLeague.Data.Access.Maps.Main
{
    public class UserSeasonLeagueMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<UserSeasonLeague>()
                .ToTable("UserSeasonLeagues")
                .HasKey(x => x.Id);
            
        }
    }
}