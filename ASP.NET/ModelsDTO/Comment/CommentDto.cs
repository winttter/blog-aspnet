namespace ASP.NET.ModelsDTO.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Guid AuthorId { get; set; }
        public string Author { get; set; }
        public int SubComments { get; set; }
    }
}
