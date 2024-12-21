namespace ASP.NET.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public int ReadingTime { get; set; }
        public string? Image { get; set; }
        public User Author { get; set; }
        public Community? Community { get; set; }
        public Guid AddressId { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Like>? Likes { get; set; }
    }
}
