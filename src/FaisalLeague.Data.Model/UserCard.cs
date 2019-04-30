using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class UserCard
    {
        public UserCard()
        {
            UserAnswers = new List<UserAnswer>();
        }
        public long Id { get; set; }
        public long UserSeasonLeagueId { get; set; }
        public virtual UserSeasonLeague UserSeasonLeague { get; set; }
        public long CardId { get; set; }
        public virtual Card Card { get; set; }
        public DateTime TimeStamp { get; set; }
        public int CardStateId { get; set; }
        public virtual CardState CardState { get; set; }

        public virtual IList<UserAnswer> UserAnswers { get; set; }

    }
}
