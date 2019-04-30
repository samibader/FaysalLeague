using System;
using System.Collections.Generic;
using System.Linq;

namespace FaisalLeague.Api.Models.Leagues
{
    public class LeagueModel
    {
        public LeagueModel()
        {
            Children = new List<LeagueModel>();
        }
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Sort { get; set; }
        public string ParentName { get; set; }
        public int ChildrenCount {
            get
            {
                return Children.Count;
            }
        }
        public virtual IList<LeagueModel> Children { get; set; }
    }
}