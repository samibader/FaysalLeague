﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FaisalLeague.Api.Models.Cities
{
    public class UpdateCityModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Sort { get; set; }
    }
}