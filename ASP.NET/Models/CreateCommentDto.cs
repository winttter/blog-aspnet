namespace ASP.NET.Models
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public Guid ParentId { get; set; }
    }
}
