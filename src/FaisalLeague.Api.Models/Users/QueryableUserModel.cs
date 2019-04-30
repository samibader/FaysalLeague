using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Api.Models.Users
{
    public class QueryableUserModel
    {
        public QueryableUserModel()
        {
            //Roles = new string[0];
            Points = 99;
            //ActiveLeague = 0;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string[] Roles { get; set; }
        public string Image { get; set; }
        public int? Points { get; set; }
        //public int ActiveLeague { get; set; }
    }
}
