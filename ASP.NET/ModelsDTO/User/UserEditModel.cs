using ASP.NET.Enums;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET.ModelsDTO.User
{
    public class UserEditModel
    {
        [Required, MaxLength(1000), MinLength(1)]
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [Required, EmailAddress, MinLength(1)]
        public string Email { get; set; }
    }
}
