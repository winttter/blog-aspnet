using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Post;
using ASP.NET.Enums;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using ASP.NET.Mappers;
using ASP.NET.ModelsDTO.Community;
using Microsoft.AspNetCore.Http.HttpResults;
//using ASP.NET.Migrations;
namespace ASP.NET.Services
{
    public interface ICommunityService
    {
        Task<List<Community>> GetCommunities();
        Task<List<CommunityUserDto>> GetMyCommunities(string userName);
        Task<CommunityFullDto> GetCommunity(Guid id);
        Task Subscribe(Guid id, string userName);
        Task Unsubscribe(Guid id, string userName);
        Task<CommunityRole?> GetRole(Guid id, string userName);
        Task<Guid> CreatePost(Guid id, string userName, CreatePostDto post);
        Task<List<PostDto>> GetPosts(string userName, Guid communityId, List<Guid>? tags, PostSorting sorting = PostSorting.CreateDesc, int page = 1, int size = 5);
    }

    public class CommunityService : ICommunityService
    {
        private readonly TestContext _context;

        public CommunityService(TestContext context)
        {
            _context = context;
        }

        public async Task<List<Community>> GetCommunities()
        {
            return await _context.Communities.ToListAsync();
        }

        public async Task<List<CommunityUserDto>> GetMyCommunities(string userName)
        {
            var userFound = await _context.Users.Include(u => u.CommunityAdmin).Include(u => u.CommunitySubscriber).FirstOrDefaultAsync(a => a.UserName == userName)!;

            var myCommunities = new List<CommunityUserDto>();

            if (!userFound.CommunitySubscriber.IsNullOrEmpty())
            {
                userFound.CommunitySubscriber.ForEach(c => myCommunities.Add(c.ToUserDto(userFound, CommunityRole.Subscriber)));
            }

            if (!userFound.CommunityAdmin.IsNullOrEmpty())
            {
                userFound.CommunityAdmin.ForEach(c => myCommunities.Add(c.ToUserDto(userFound, CommunityRole.Administrator)));
            }

            return myCommunities;
        }

        public async Task<CommunityFullDto> GetCommunity(Guid id)
        {
            var community = await _context.Communities.Include(c => c.Administrators)
                .Include(c => c.Subscribers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (community == null)
            {
                throw new Exception("404*community not found");
            }

            return community.ToFullDto(community.Subscribers.Count());
        }

        public async Task Subscribe(Guid id, string userName)
        {
            var communityFound = await _context.Communities
                .Include(c => c.Subscribers)
                .FirstOrDefaultAsync(c => c.Id == id);

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            if (communityFound == null)
            {
                throw new Exception("404*Community does not exist");
            }
            else if ((!userFound.CommunityAdmin.IsNullOrEmpty() && userFound.CommunityAdmin.Any(c => c.Id == communityFound.Id)))
            {
                throw new Exception("400*user is an admin of this community");
            }
            else if (!userFound.CommunitySubscriber.IsNullOrEmpty() && userFound.CommunitySubscriber.Any(c => c.Id == communityFound.Id))
            {
                throw new Exception("400*user already subcribed");
            }

            userFound.CommunitySubscriber.Add(communityFound);
            communityFound.Subscribers.Add(userFound);

            await _context.SaveChangesAsync();
        }

        public async Task Unsubscribe(Guid id, string userName)
        {
            var communityFound = await _context.Communities
                .Include(c => c.Subscribers)
                .FirstOrDefaultAsync(c => c.Id == id);

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            if (communityFound == null)
            {
                throw new Exception("404*Community does not exist");
            }
            else if (userFound.CommunitySubscriber.IsNullOrEmpty() || !userFound.CommunitySubscriber.IsNullOrEmpty() && !userFound.CommunitySubscriber.Any(c => c.Id == communityFound.Id))
            {
                throw new Exception("400*user has not subcribed");
            }

            userFound.CommunitySubscriber.Remove(communityFound);
            communityFound.Subscribers.Remove(userFound);

            await _context.SaveChangesAsync();
        }

        public async Task<CommunityRole?> GetRole(Guid id, string userName)
        {
            var communityFound = await _context.Communities
                .FirstOrDefaultAsync(c => c.Id == id);

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            if (userFound.CommunityAdmin != null && userFound.CommunityAdmin.Any(c => c == communityFound))
            {
                return CommunityRole.Administrator;
            }

            if (userFound.CommunitySubscriber != null && userFound.CommunitySubscriber.Any(c => c == communityFound))
            {
                return CommunityRole.Subscriber;
            }

            return null;
        }
         
        public async Task<Guid> CreatePost(Guid id, string userName, CreatePostDto model)
        {
            var communityFound = await _context.Communities
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Id == id);

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            if (communityFound == null)
            {
                throw new Exception("404*Community not found");
            }
            else if (!(userFound.CommunityAdmin != null && userFound.CommunityAdmin.Any(c => c == communityFound)))
            {
                throw new Exception("403*Action not allowed");
            }
            else if (model.Tags.IsNullOrEmpty()) { 
                throw new Exception("400*No tags");
            }

            Post newPost = new Post();
            newPost.Id = Guid.NewGuid();
            newPost.CreateTime = DateTime.Now;
            newPost.Title = model.Title;
            newPost.Description = model.Description;
            newPost.ReadingTime = model.ReadingTime;
            newPost.Image = model.Image;
            newPost.AddressId = model.AddressId;
            newPost.Community = communityFound;
            newPost.Author = userFound;
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

            communityFound.Posts.Add(newPost);

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return newPost.Id;
        }

        public async Task<List<PostDto>> GetPosts(string userName, Guid communityId, List<Guid>? tags, PostSorting sorting = PostSorting.CreateDesc, int page = 1, int size = 5)
        {
            var communityFound = await _context.Communities
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Id == communityId);

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            if (communityFound == null)
            {
                throw new Exception("404*Community not found");
            }
            else if (!(userFound.CommunityAdmin != null && userFound.CommunityAdmin.Any(c => c == communityFound)))
            {
                throw new Exception("403*Action not allowed");
            }

            var listOfPosts = _context.Posts.Include(p => p.Likes)
                .ThenInclude(l => l.Liker)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .Include(p => p.Author)
                .Include(p => p.Community)
                .Select(p => p);

            listOfPosts = listOfPosts.Where(p => p.Community == communityFound);

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

            var result = await listOfPosts.Skip((page - 1) * size).Take(size).ToListAsync();

            return result.ToDtos(userFound);
        }
    }
}
