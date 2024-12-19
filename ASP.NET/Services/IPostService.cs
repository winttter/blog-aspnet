using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Post;
using ASP.NET.Enums;
using System.Linq;
//using ASP.NET.Migrations;
namespace ASP.NET.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPosts(string userName, List<Tag> tags, string author, int min, int max, PostSorting sorting, bool onlyMyCommunities, int page, int size);
        Task<string> PostPost(CreatePostDto model, string userName);
    }

    public class PostService : IPostService
    {
        //обращение к БД
        private readonly TestContext _context;

        public PostService(TestContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPosts(string userName, List<Tag> tags, string author, int min, int max, PostSorting sorting, bool onlyMyCommunities, int page, int size)
        {
            var userFound = _context.Users.FirstOrDefault(a => a.UserName == userName)!;

            var listOfPosts = _context.Posts
                .Where(post => post.ReadingTime >= min && post.ReadingTime <= max && post.Author.Contains(author));

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

            var result = await listOfPosts.ToListAsync();

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
    }
}
