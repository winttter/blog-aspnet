using ASP.NET.Enums;

namespace ASP.NET.ModelsDTO.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
