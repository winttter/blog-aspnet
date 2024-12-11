using System.ComponentModel.DataAnnotations;

namespace ASP.NET.Models
{
    public class UserRegisterModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
