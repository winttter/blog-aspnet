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

namespace ASP.NET.Controllers
{
    [Route("api/post")]
    [ApiController]

    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Post(CreatePostDto model)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _postService.PostPost(model, name));
        }


        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery]
            List<Guid>? tags,
            string? author,
            [Range(0, int.MaxValue, ErrorMessage = "Minimum reading time cannot be what you sent me.")]
            int? min,
            [Range(0, int.MaxValue, ErrorMessage = "Max reading time cannot be what you sent me.")]
            int? max,
            PostSorting sorting = PostSorting.CreateDesc,
            bool onlyMyCommunities = false,
            [Range(1, int.MaxValue, ErrorMessage = "page number cannot be what you sent me.")]
            int page = 1,
            [Range(1, int.MaxValue, ErrorMessage = "page size cannot be what you sent me.")]
            int size = 5)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _postService.GetPosts(name, tags, author, min, max, sorting, onlyMyCommunities, page, size));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _postService.GetPost(id, name));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{postId}")]
        public async Task<IActionResult> LikePost(Guid postId)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            await _postService.LikePost(postId, name);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> RemoveLikePost(Guid postId)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            await _postService.RemoveLikePost(postId, name);
            return Ok();
        }
    }
}
