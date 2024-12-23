using ASP.NET.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace ASP.NET.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string FullName { get; set; }
        public List<Post> Posts { get; set; }
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Community>? CommunityAdmin { get; set; }
        public List<Community>? CommunitySubscriber { get; set; }
        
    }
}
