using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Post;
using ASP.NET.Enums;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using ASP.NET.Mappers;
using ASP.NET.ModelsDTO.Comment;
using Microsoft.AspNetCore.Http.HttpResults;
//using ASP.NET.Migrations;
namespace ASP.NET.Services
{
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAuthorList();
    }

    public class AuthorService : IAuthorService
    {
        //обращение к БД
        private readonly TestContext _context;

        public AuthorService(TestContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorDto>> GetAuthorList()
        {
            var authors = await _context.Users
                .Include(u => u.Likes)
                .Include(u => u.Posts)
                .Where(u => _context.Posts.Any(p => p.AuthorId == u.Id)).ToListAsync();

            return authors.ToAuthorDtos();
        }

        
    }
}
