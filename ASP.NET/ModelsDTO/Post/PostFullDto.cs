using ASP.NET.Models;
using ASP.NET.ModelsDTO.Comment;

namespace ASP.NET.ModelsDTO.Post
{
    public class PostFullDto
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
        public List<TagDto> Tags { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
