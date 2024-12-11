namespace ASP.NET.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public int ReadingTime { get; set; }
        public string? Image { get; set; }
        public Guid AuthorId { get; set; }
        public string Author { get; set; }
        public Guid? CommunityId { get; set; }
        public string? CommunityName { get; set; }
        public Guid AddressId { get; set; }
        public int Likes { get; set; }
        public bool? HasLike { get; set; }
        public int CommentsCount { get; set; }
        public List<Tag> Tags { get; set; }

        public Post(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}
