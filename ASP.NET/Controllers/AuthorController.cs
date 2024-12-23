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
    [Route("api/author")]
    [ApiController]

    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthorList()
        {
            return Ok(await _authorService.GetAuthorList());
        }
    }
}
