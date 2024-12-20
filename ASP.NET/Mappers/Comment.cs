using ASP.NET.Models;
using ASP.NET.ModelsDTO.Comment;

namespace ASP.NET.Mappers
{
    public static class CommentsMapper
    {
        public static CommentDto ToDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                CreateTime = comment.CreateTime,
                Content = comment.Content,
                DeleteDate = comment.DeleteDate,
                ModifiedDate = comment.ModifiedDate,
                AuthorId = comment.AuthorId,
                Author = comment.Author,

            };
        }

        public static List<CommentDto> ToDtos(this List<Comment> comments)
        {
            return comments.Select(ToDto).ToList();
        }
    }
}
