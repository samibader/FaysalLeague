using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Access.Maps.Main
{
    public class CityMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<City>()
                .ToTable("Cities")
                .HasKey(x => x.Id);

            builder.Entity<City>()
            .Property(b => b.Name)
            .HasMaxLength(500).IsRequired();

        }
    }
}
