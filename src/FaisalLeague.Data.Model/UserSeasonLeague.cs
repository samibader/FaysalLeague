using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class UserSeasonLeague
    {
        public UserSeasonLeague()
        {

        }

        public long Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public long SeasonId { get; set; }
        public virtual Season Season { get; set; }
        public long LeagueId { get; set; }
        public virtual League League { get; set; }

        public bool IsActive { get; set; }
        public DateTime SubscriptionDateTime { get; set; }

        public int Points { get; set; }

    }
}
