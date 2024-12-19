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

namespace ASP.NET.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Get(List<Tag> tags, string author, int min, int max, PostSorting sorting, bool onlyMyCommunities, int page, int size)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _postService.GetPosts(name, tags, author, min, max, sorting, onlyMyCommunities, page, size));
        }

    }
}
