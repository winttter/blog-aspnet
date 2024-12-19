using ASP.NET.Enums;

namespace ASP.NET.ModelsDTO.Community
{
    public class CommunityUserDto
    {
        public Guid UserId { get; set; }
        public Guid CommunityId { get; set; }
        public CommunityRole Role { get; set; }
    }
}
