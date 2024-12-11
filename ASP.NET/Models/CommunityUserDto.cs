using ASP.NET.Enums;

namespace ASP.NET.Models
{
    public class CommunityUserDto
    {
        public Guid UserId { get; set; }
        public Guid CommunityId { get; set; }
        public CommunityRole Role { get; set; }
    }
}
