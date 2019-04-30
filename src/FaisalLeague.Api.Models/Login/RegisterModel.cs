using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Model;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FaisalLeague.Api.Models.Users.Validation;

namespace FaisalLeague.Api.Models.Login
{
    public class RegisterModel
    {
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
        [EmailAddress]
        [UniqueEmail]
        public string Email { get; set; }
        [Required]
        [UniqueMobile]
        public string Mobile { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Invalid date format.")]
        public string DOB { get; set; }

    }
}