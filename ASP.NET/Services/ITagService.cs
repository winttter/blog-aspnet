using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Post;
using ASP.NET.Enums;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using ASP.NET.Mappers;
using System.Threading.Tasks;
//using ASP.NET.Migrations;
namespace ASP.NET.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetTags();
        Task PostTags(List<Tag> tags);
    }

    public class TagService : ITagService
    {
        //обращение к БД
        private readonly TestContext _context;

        public TagService(TestContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetTags()
        {
           return await _context.Tags.ToListAsync();
        }

        public async Task PostTags(List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                _context.Tags.Add(tag);
            }

            await _context.SaveChangesAsync();
        }
    }
}
