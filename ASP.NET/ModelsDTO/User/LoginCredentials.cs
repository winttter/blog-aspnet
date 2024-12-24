using System.ComponentModel.DataAnnotations;

namespace ASP.NET.ModelsDTO.User
{
    public class LoginCredentials
    {
        [Required]
        [EmailAddress]
        [MinLength(1)]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
