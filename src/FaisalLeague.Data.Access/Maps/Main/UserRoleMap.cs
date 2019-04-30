using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Data.Access.Maps.Main
{
    public class UserRoleMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<UserRole>()
                .ToTable("UserRoles")
                .HasKey(x => x.Id);
            
        }
    }
}