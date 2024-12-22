using ASP.NET.Models;
using ASP.NET.ModelsDTO.Post;
using ASP.NET.ModelsDTO.User;
using Microsoft.IdentityModel.Tokens;

namespace ASP.NET.Mappers
{
    public static class PostMapper
    {
        public static PostDto ToDto(this Post post, User user)
        {
            var hasLike = false;

            if (!post.Likes.IsNullOrEmpty())
            {
                hasLike = post.Likes.Any(l => l.Liker == user);
            }



            var newPostDto = new PostDto
            {
                Id = post.Id,
                CreateTime = post.CreateTime,
                Title = post.Title,
                Description = post.Description,
                Tags = post.Tags.ToDtos(),
                CommentsCount = post.Comments.Count(),
                ReadingTime = post.ReadingTime,
                AuthorId = post.Author.Id,
                Author = post.Author.FullName,
                AddressId = post.AddressId,
                Likes = post.Likes.Count(),
                HasLike = hasLike,
                Image = post.Image
            };

            if (post.Community != null)
            {
                newPostDto.CommunityId = post.Community.Id;
                newPostDto.CommunityName = post.Community.Name;
            }

            return newPostDto;
        }

        public static List<PostDto> ToDtos(this List<Post> posts, User user)
        {
            return posts.Select(p => p.ToDto(user)).ToList();
        }
    }
}
