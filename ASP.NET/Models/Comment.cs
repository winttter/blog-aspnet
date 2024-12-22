namespace ASP.NET.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Guid AuthorId { get; set; }
        public User Author { get; set; }
        public Guid ParentPostId { get; set; }
        public Post ParentPost { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
