using System;
using System.Collections.Generic;

namespace FaisalLeague.Data.Model
{
    public class User
    {
        public User()
        {
            Roles = new List<UserRole>();
            Image = "defaultUser.jpg";
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DOB { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public virtual IList<UserRole> Roles { get; set; }
        public IList<UserSeasonLeague> UserSeasonLeagues { get; set; }
        
    }
}
