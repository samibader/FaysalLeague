using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Data.Access.Maps.Main
{
    public class RoleMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<Role>()
                .ToTable("Roles")
                .HasKey(x => x.Id);
            
        }
    }
}