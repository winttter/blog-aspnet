using ASP.NET.Enums;

namespace ASP.NET.ModelsDTO.User
{
    public class UserEditModel
    {
        public string FullName { get; set; }
        //[Required]
        public DateTime? BirthDate { get; set; }
        public Gender Gender { get; set; }
        //[Required]
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        //[Required]
    }
}
