using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;
using ASP.NET.Migrations;
namespace ASP.NET.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPosts();
    }

    public class PostService : IPostService
    {
        //обращение к БД
        private readonly TestContext _context;

        public PostService(TestContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPosts()
        {
            var listOfPosts = await _context.Posts.ToListAsync();
            return listOfPosts;
        }
    }
}
