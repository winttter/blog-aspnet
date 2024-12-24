using System.ComponentModel.DataAnnotations;

namespace ASP.NET.ModelsDTO.Comment
{
    public class UpdateCommentDto
    {
        [MinLength(1), MaxLength(1000)]
        public string Content { get; set; }
    }
}
