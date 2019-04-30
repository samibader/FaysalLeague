using System;
using System.Collections.Generic;
using System.Linq;

namespace FaisalLeague.Api.Models.Leagues
{
    public class LeagueBasicInfoModel
    {
        public LeagueBasicInfoModel()
        {
            
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Sort { get; set; }
        public string ParentName { get; set; }
    }
}