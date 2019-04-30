using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FaisalLeague.Data.Model
{
    public class UserConnection
    {
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual User User { get; set; }

    }
}
