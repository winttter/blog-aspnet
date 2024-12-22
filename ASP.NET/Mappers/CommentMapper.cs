using ASP.NET.Models;
using ASP.NET.ModelsDTO.Comment;

namespace ASP.NET.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToDto(this Comment comment, List<Comment> comments)
        {
            return new CommentDto
            {
                Id = comment.Id,
                CreateTime = comment.CreateTime,
                Content = comment.Content,
                DeleteDate = comment.DeleteDate,
                ModifiedDate = comment.ModifiedDate,
                AuthorId = comment.Author.Id,
                Author = comment.Author.FullName,
                SubComments = comments.Count(c => comment.Id == c.ParentCommentId)
            };
        }

        public static List<CommentDto> ToDtos(this List<Comment> comments)
        {
            return comments.Select(c => c.ToDto(comments)).ToList();
        }
    }
}
