using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Post;
using ASP.NET.Enums;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using ASP.NET.Mappers;
//using ASP.NET.Migrations;
namespace ASP.NET.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPosts(string userName, List<Tag>? tags, string? author, int? min, int? max, PostSorting sorting, bool onlyMyCommunities, int page, int size);
        Task<string> PostPost(CreatePostDto model, string userName);
        Task<PostFullDto> GetPost(Guid postId);
    }

    public class PostService : IPostService
    {
        //обращение к БД
        private readonly TestContext _context;

        public PostService(TestContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPosts(string userName, List<Tag>? tags, string? author, int? min, int? max, PostSorting sorting = PostSorting.CreateDesc, bool onlyMyCommunities = false, int page = 1, int size = 5)
        {
            var userFound = _context.Users.FirstOrDefault(a => a.UserName == userName)!;


            var listOfPosts = _context.Posts.Select(p => p);

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
                listOfPosts = listOfPosts.Where(post => post.Author.Contains(author));
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
                    listOfPosts = listOfPosts.OrderBy(post => post.Likes);
                    break;
                case PostSorting.LikeDesc:
                    listOfPosts = listOfPosts.OrderByDescending(post => post.Likes);
                    break;
            }

            listOfPosts = listOfPosts.Skip((page - 1) * size).Take(size);

            var result = await listOfPosts.ToListAsync();
            List<Post> postsFiltered = new List<Post>();

            if (!tags.IsNullOrEmpty())
            {
                foreach (var post in listOfPosts)
                {
                    bool flag = false;
                    foreach (var tag in post.Tags)
                    {
                        foreach (var tagNeeded in tags)
                        {
                            if (tagNeeded.Name == tag.Name)
                            {
                                flag = true;
                                postsFiltered.Add(post);
                                break;
                            }
                        }

                        if (flag)
                        {
                            break;
                        }
                    }
                }

                result = postsFiltered;
            }

            

            return result;
        }

        public async Task<string> PostPost(CreatePostDto model, string userName)
        {
            Post newPost = new Post();
            newPost.Id = Guid.NewGuid();
            newPost.CreateTime = DateTime.Now;
            newPost.Title = model.Title;
            newPost.Description = model.Description;
            newPost.ReadingTime = model.ReadingTime;
            newPost.Image = model.Image;
            newPost.AddressId = model.AddressId;
            newPost.Tags = model.Tags;

            var userFound = _context.Users.FirstOrDefault(a => a.UserName == userName)!;

            newPost.Author = userFound.UserName;
            newPost.AuthorId = userFound.Id;

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return newPost.Id.ToString();
        }

        public async Task<PostFullDto> GetPost(Guid postId)
        {
            var postFound = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Tags)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId);

            var fullPost = new PostFullDto();

            fullPost.Id = postFound.Id;
            fullPost.CreateTime = DateTime.Now;
            fullPost.Title = postFound.Title;
            fullPost.Description = postFound.Description;
            fullPost.CommunityName = postFound.CommunityName;
            fullPost.Author = postFound.Author;
            fullPost.AuthorId = postFound.AuthorId;
            fullPost.AddressId = postFound.AddressId;
            fullPost.CommunityId = postFound.CommunityId;
            fullPost.Comments = postFound.Comments.ToDtos();
            fullPost.Likes = postFound.Likes;
            //fullPost.Tags = postFound.Tags.ToDtos();

            return fullPost;
        }
    }
}
