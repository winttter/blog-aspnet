using ASP.NET.Enums;

namespace ASP.NET.Models
{
    public class AuthorDto
    {
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int Posts { get; set; }
        public int Likes { get; set; }
        public DateTime Created { get; set; }
    }
}
