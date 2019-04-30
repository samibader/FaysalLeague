using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class City
    {
        public City()
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public bool IsDeleted { get; set; }
        public virtual IList<User> Users { get; set; }
    }
}
