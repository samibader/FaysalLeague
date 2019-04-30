using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class League
    {
        public League()
        {
            Image = "defaultLeague.jpg";
        }

        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Sort { get; set; }

        [ForeignKey("ParentId")]
        public virtual League Parent { get; set; }

        public virtual ICollection<League> Children { get; set; }
    }
}
