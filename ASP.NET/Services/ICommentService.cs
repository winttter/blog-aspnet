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
    public interface ICommentService
    {
        Task CommentPost(Guid postId, string userName, CreateCommentDto model);
        Task DeleteComment(Guid commentId, string userName);
        Task <List<CommentDto>> GetReplies(Guid commentId, string userName);
    }

    public class CommentService : ICommentService
    {
        //обращение к БД
        private readonly TestContext _context;

        public CommentService(TestContext context)
        {
            _context = context;
        }

        
        public async Task CommentPost(Guid postId, string userName, CreateCommentDto model)
        {
            var postFound = await _context.Posts
                .Include(p => p.Comments)
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
                throw new Exception("403*user has no rights to comment this post");
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
                Content = model.Content,
                ParentPostId = postFound.Id,
                ParentPost = postFound,
                AuthorId = userFound.Id,
                Author = userFound,
                ParentCommentId = model.ParentId,
            };

            if (comment.ParentCommentId != null)
            {
                if (!(_context.Comments.Include(c => c.ParentPost)
                    .First(c => c.Id == comment.ParentCommentId).ParentPost == comment.ParentPost)) {
                    throw new Exception("400*Parent posts don't match");
                }
            }

            await _context.Comments.AddAsync(comment);

            _context.SaveChanges();
        }

        public async Task DeleteComment(Guid commentId, string userName)
        {
            var commentFound = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.ParentPost)
                .ThenInclude(p => p.Community)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            if (commentFound == null)
            {
                throw new Exception("404*comment does not exist");
            }
            else if (commentFound.Author != userFound)
            {
                throw new Exception("403*user is not the author of this comment");
            }
            if (commentFound.ParentPost.Community != null && commentFound.ParentPost.Community.IsClosed && !userFound.CommunityAdmin.Contains(commentFound.ParentPost.Community) && !userFound.CommunitySubscriber.Contains(commentFound.ParentPost.Community))
            {
                throw new Exception("403*user has no rights to delete comment");
            }

            if (_context.Comments.Any(c => c.ParentCommentId == commentFound.Id))
            {
                commentFound.DeleteDate = DateTime.UtcNow;
                commentFound.Content = "";
            }
            else
            {
                _context.Comments.Remove(commentFound);

                if (_context.Comments.Any(c => c.Id == commentFound.ParentCommentId && c.DeleteDate != null))
                {
                    _context.Comments.Remove(_context.Comments.First(c => c.Id == commentFound.ParentCommentId && c.DeleteDate != null));
                }
            }
            
            await _context.SaveChangesAsync();

        }

        public async Task<List<CommentDto>> GetReplies(Guid commentId, string userName)
        {
            var commentFound = await _context.Comments
               .Include(c => c.Author)
               .Include(c => c.ParentPost)
               .ThenInclude(p => p.Community)
               .FirstOrDefaultAsync(c => c.Id == commentId);

            if (commentFound == null) throw new Exception("404*Comment not found");

            var userFound = _context.Users
                .Include(u => u.CommunityAdmin)
                .Include(u => u.CommunitySubscriber)
                .FirstOrDefault(a => a.UserName == userName)!;

            var commentsList = new List<Comment>
            {
                commentFound
            };

            var answerCommentsList = new List<Comment>();

            while (!commentsList.IsNullOrEmpty())
            {
                var currentComment = commentsList[0];
                commentsList.RemoveAt(0);

                if (_context.Comments.Any(c => c.ParentCommentId == currentComment.Id))
                {
                    foreach (var child in _context.Comments.Where(c => c.ParentCommentId == currentComment.Id).ToList())
                    {
                        commentsList.Add(child);
                        answerCommentsList.Add(child);
                    }
                }
            }

            return answerCommentsList.ToDtos();

        }
    }
}
