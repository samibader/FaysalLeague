using System.ComponentModel.DataAnnotations;

namespace FaisalLeague.Api.Models.Users
{
    public class ChangeUserPasswordModel
    {
        [Required]
        public string Password { get; set; }
    }
}