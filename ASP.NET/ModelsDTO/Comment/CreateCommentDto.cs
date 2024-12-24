using System.ComponentModel.DataAnnotations;

namespace ASP.NET.ModelsDTO.Comment
{
    public class CreateCommentDto
    {
        [MinLength(1), MaxLength(1000)]
        public string Content { get; set; }
        public Guid? ParentId { get; set; }
    }
}
