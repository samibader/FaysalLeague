using FaisalLeague.Api.Models.Users.Validation;
using System.ComponentModel.DataAnnotations;

namespace FaisalLeague.Api.Models.Users
{
    public class UpdateUserModel
    {
        public UpdateUserModel()
        {
            //Roles = new string[0];
        }

        [Required]
        [UniqueUsername]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [UniqueEmail]
        public string Email { get; set; }
        [Required]
        [UniqueMobile]
        public string Mobile { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string DOB { get; set; }
        //public string[] Roles { get; set; }
    }
}