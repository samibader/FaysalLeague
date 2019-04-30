using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Api.Models.Users
{
    public class UserActiveLeague
    {
        public int UserId { get; set; }
        public long SeasonId { get; set; }
        public long LeagueId { get; set; }
        public string SubscriptionDateTime { get; set; }
    }
}
