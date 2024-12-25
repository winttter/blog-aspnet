using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Post;
using ASP.NET.Enums;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using ASP.NET.Mappers;
using ASP.NET.Helper;
//using ASP.NET.Migrations;
namespace ASP.NET.Services
{
    public interface IPostService
    {
        Task<PostPagedList> GetPosts(string userName, List<Guid>? tags, string? author, int? min, int? max, PostSorting sorting, bool onlyMyCommunities, int page, int size);
        Task<string> PostPost(CreatePostDto model, string userName);
        Task<PostFullDto> GetPost(Guid postId, string userName);
        Task LikePost(Guid postId, string userName);
        Task RemoveLikePost(Guid postId, string userName);
    }

    public class PostService : IPostService
    {
        private readonly TestContext _context;

        public PostService(TestContext context)
        {
            _context = context;
        }

        public async Task<PostPagedList> GetPosts(string userName, List<Guid>? tags, string? author, int? min, int? max, PostSorting sorting = PostSorting.CreateDesc, bool onlyMyCommunities = false, int page = 1, int size = 5)
        {
            var userFound = _context.Users.FirstOrDefault(a => a.UserName == userName)!;

            if (max < min)
            {
                throw new Exception("400*max mustn't be less than min");
            }

            foreach (var tag in tags)
            {
                var foundTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tag);

                if (foundTag == null)
                {
                    throw new Exception("404*Tag not found");
                }
            }

            var listOfPosts = _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Tags)
                .Include(p => p.Author)
                .Include(p => p.Community)
                .Include(p => p.Likes).ThenInclude(l => l.Liker).Select(p => p);

            if (min != null)
            {
                listOfPosts = listOfPosts.Where(post => post.ReadingTime >= min);
            }

            if (max != null)
            {
                listOfPosts = listOfPosts.Where(post => post.ReadingTime <= max);
            }

            if (author != null)
            {
                listOfPosts = listOfPosts.Where(post => post.Author.FullName.Contains(author));
            }

                switch (sorting)
            {
                case PostSorting.CreateDesc: 
                    listOfPosts = listOfPosts.OrderByDescending(post => post.CreateTime);
                    break;
                case PostSorting.CreateAsc:
                    listOfPosts = listOfPosts.OrderBy(post => post.CreateTime);
                    break;
                case PostSorting.LikeAsc:
                    listOfPosts = listOfPosts.OrderBy(post => post.Likes.Count());
                    break;
                case PostSorting.LikeDesc:
                    listOfPosts = listOfPosts.OrderByDescending(post => post.Likes.Count());
                    break;
            }

            if (!tags.IsNullOrEmpty())
            {
                listOfPosts = listOfPosts.Where(p => p.Tags.Any(t => tags.Contains(t.Id)));
            }

            int countList = listOfPosts.Count();

            var result = await listOfPosts.Skip((page - 1) * size).Take(size).ToListAsync();
            var answer = new PostPagedList();
            answer.Posts = result.ToDtos(userFound);
            answer.Pagination = new PageInfoModel()
            {
                Count = countList % size > 0 ? countList / size + 1 : countList / size,
                Current = page,
                Size = size
            };

            return answer;
        }

        public async Task<string> PostPost(CreatePostDto model, string userName)
        {
            if (!await IHelper.IsImageUrlValidAsync(model.Image))
            {
                throw new Exception("400*image is not valid");
            }

            Post newPost = new Post();
            newPost.Id = Guid.NewGuid();
            newPost.CreateTime = DateTime.Now;
            newPost.Title = model.Title;
            newPost.Description = model.Description;
            newPost.ReadingTime = model.ReadingTime;
            newPost.Image = model.Image;
            newPost.AddressId = model.AddressId;
            newPost.Tags = new List<Tag>();

            foreach (var tag in model.Tags)
            {
                var foundTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tag);

                if (foundTag == null)
                {
                    throw new Exception("404*Tag not found");
                }

                newPost.Tags.Add(foundTag);
            }

            var userFound = _context.Users.FirstOrDefault(a => a.UserName == userName)!;

            newPost.Author = userFound;

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return newPost.Id.ToString();
        }

        public async Task<PostFullDto> GetPost(Guid postId, string userName)
        {
            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;


            var postFound = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Tags)
                .Include(p => p.Likes)
                .Include(p => p.Author)
                .Include(p => p.Community)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (postFound == null)
            {
                throw new Exception("404*post does not exist");
            }

            var fullPost = new PostFullDto();

            fullPost.Id = postFound.Id;
            fullPost.CreateTime = DateTime.Now;
            fullPost.Title = postFound.Title;
            fullPost.Description = postFound.Description;

            if (postFound.Community != null && postFound.Community.IsClosed && !userFound.CommunityAdmin.Contains(postFound.Community) && !userFound.CommunitySubscriber.Contains(postFound.Community))
            {
                throw new Exception("403*user has no rights to view this post");
            }
            else if (postFound.Community != null)
            {
                fullPost.CommunityName = postFound.Community.Name;
                fullPost.CommunityId = postFound.Community.Id;
            }
            fullPost.Author = postFound.Author.FullName;
            fullPost.AuthorId = postFound.Author.Id;
            fullPost.AddressId = postFound.AddressId;
            
            fullPost.Comments = postFound.Comments.ToDtos();
            fullPost.Comments = fullPost.Comments.Where(c => postFound.Comments.First(com => com.Id == c.Id)!.ParentCommentId == null).ToList();
            fullPost.Likes = postFound.Likes.Count();
            fullPost.Tags = postFound.Tags.ToDtos();

            return fullPost;
        }

        public async Task LikePost(Guid postId, string userName)
        {
            var postFound = await _context.Posts
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId);

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            if (postFound == null)
            {
                throw new Exception("404*Post does not exist");
            }
            else if (postFound.Community != null && postFound.Community.IsClosed && !userFound.CommunityAdmin.Contains(postFound.Community) && !userFound.CommunitySubscriber.Contains(postFound.Community))
            {
                throw new Exception("403*user has no rights to view this post");
            }
            else if (!postFound.Likes.IsNullOrEmpty() && postFound.Likes.Any(l => l.Liker.Id == userFound.Id))
            {
                throw new Exception("400*user already liked this post");
            }

            var like = new Like
            {
                Id = postFound.Id,
                LikedPostId = postFound.Id,
                LikedPost = postFound,
                LikerId = userFound.Id,
                Liker = userFound
            };

            if (postFound.Likes == null)
            {
                postFound.Likes = new List<Like>();
            }
          
            await _context.Likes.AddAsync(like);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveLikePost(Guid postId, string userName)
        {
            var postFound = await _context.Posts
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId);

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            if (postFound == null)
            {
                throw new Exception("404*Post does not exist");
            }
            else if (!(!postFound.Likes.IsNullOrEmpty() && postFound.Likes.Any(l => l.Liker.Id == userFound.Id)))
            {
                throw new Exception("400*user has not liked this post yet");
            }

            var likeToRemove = await _context.Likes.FirstOrDefaultAsync(l => l.Liker.Id == userFound.Id);

            if (likeToRemove == null) throw new Exception("404*user has not liked this post yet");

            _context.Likes.Remove(likeToRemove);

            await _context.SaveChangesAsync();
        }
    }
}
