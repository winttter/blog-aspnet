using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP.NET.Models;
using ASP.NET.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ASP.NET.ModelsDTO.Post;
using System.Security.Claims;
using ASP.NET.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASP.NET.ModelsDTO.Comment;

namespace ASP.NET.Controllers
{
    [Route("api/comment")]
    [ApiController]

    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{postId}")]
        public async Task<IActionResult> CommentPost(Guid postId, CreateCommentDto model)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            await _commentService.CommentPost(postId, name, model);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            await _commentService.DeleteComment(commentId, name);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetReplies(Guid commentId)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            
            return Ok(await _commentService.GetReplies(commentId, name));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{commentId}")]
        public async Task<IActionResult> EditComment(Guid commentId, UpdateCommentDto comment)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            await _commentService.EditComment(commentId, name, comment);

            return Ok();
        }
    }
}
