namespace ASP.NET.ModelsDTO.Comment
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public Guid? ParentId { get; set; }
    }
}
