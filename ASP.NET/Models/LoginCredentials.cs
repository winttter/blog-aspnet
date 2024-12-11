﻿using System.ComponentModel.DataAnnotations;

namespace ASP.NET.Models
{
    public class LoginCredentials
    {
        [Required]
        [EmailAddress]
        [MinLength(1)]
        public string Email { get; set; }
        [Required]
        [MinLength(1)]
        public string Password { get; set; }
    }
}
