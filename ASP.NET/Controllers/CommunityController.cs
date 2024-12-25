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
using System.Drawing;

namespace ASP.NET.Controllers
{
    [Route("api/community")]
    [ApiController]

    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityService;

        public CommunityController(ICommunityService communityService)
        {
            _communityService = communityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommunities()
        {
            return Ok(await _communityService.GetCommunities());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyCommunities()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _communityService.GetMyCommunities(name));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunity(Guid id)
        {
            return Ok(await _communityService.GetCommunity(id));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{communityId}")]
        public async Task<IActionResult> Subscribe(Guid communityId)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            await _communityService.Subscribe(communityId, name);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{communityId}")]
        public async Task<IActionResult> Unsubscribe(Guid communityId)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            await _communityService.Unsubscribe(communityId, name);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{communityId}/role")]
        public async Task<IActionResult> GetRole(Guid communityId)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _communityService.GetRole(communityId, name));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{communityId}/post")]
        public async Task<IActionResult> CreatePost(Guid communityId, [FromBody] CreatePostDto post)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _communityService.CreatePost(communityId, name, post));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{communityId}/post")]
        public async Task<IActionResult> GetPosts(Guid communityId, [FromQuery] List<Guid>? tags, PostSorting sorting = PostSorting.CreateDesc,[Range(1, int.MaxValue)] int page = 1, [Range(1, int.MaxValue)] int size = 5)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _communityService.GetPosts(name, communityId, tags, sorting, page, size));
        }
    }
}
