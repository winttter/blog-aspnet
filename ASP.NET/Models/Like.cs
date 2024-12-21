namespace ASP.NET.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid LikerId { get; set; }
        public User Liker { get; set; }
        public Guid LikedPostId { get; set; }
        public Post LikedPost { get; set; }
    }
}
