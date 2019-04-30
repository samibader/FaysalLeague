using System.Collections.Generic;

namespace FaisalLeague.Api.Models.Users
{
    public class UserModel
    {
        public UserModel()
        {
            //Roles = new string[0];
            //Points = 99;
            //ActiveLeague = 0;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        //public string[] Roles { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }
        public int? Points { get; set; }
        //public long ActiveLeague { get; set; }
    }
}