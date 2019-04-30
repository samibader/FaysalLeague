using System;

namespace FaisalLeague.Api.Models.Seasons
{
    public class SeasonModel
    {
        public long Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
    }
}