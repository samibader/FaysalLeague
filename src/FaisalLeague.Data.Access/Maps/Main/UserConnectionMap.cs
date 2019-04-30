using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;


namespace FaisalLeague.Data.Access.Maps.Main
{
    public class UserConnectionMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<UserConnection>()
                .ToTable("UserConnections")
                .HasKey(c => new { c.UserId, c.ConnectionId });

        }
    }
}