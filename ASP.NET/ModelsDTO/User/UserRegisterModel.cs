using ASP.NET.Enums;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET.ModelsDTO.User
{
    public class UserRegisterModel
    {
        //[Required]
        public string FullName { get; set; }
        //[Required]
        public DateTime? BirthDate { get; set; }
        //[Required]
        public Gender Gender { get; set; }
        //[Required]
        public string? PhoneNumber { get; set; }
        //[Required]
        public string Email { get; set; }
        //[Required]
        public string Password { get; set; }
    }
}
