using System;
using System.ComponentModel.DataAnnotations;

namespace FaisalLeague.Api.Models.Seasons
{
    public class CreateSeasonModel
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Description { get; set; }
    }
}