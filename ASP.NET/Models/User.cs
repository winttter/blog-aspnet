using Microsoft.AspNetCore.Identity;

namespace ASP.NET.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        //[Required]
        public string Password { get; set; }
        public List<Community>? CommunityAdmin { get; set; }
        public List<Community>? CommunitySubscriber { get; set; }
        /*public User(UserRegisterModel newUser)
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            FullName = newUser.FullName;
            BirthDate = newUser.BirthDate;
            Gender = newUser.Gender;
            PhoneNumber = newUser.PhoneNumber;
            CommunityAdmin = [];
            CommunitySubscriber = [];
        }*/
    }
}
