using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP.NET.Services;
using ASP.NET.ModelsDTO.User;
using ASP.NET.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            return Ok(await _userService.Register(model));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCredentials model)
        {
            return Ok(await _userService.Login(model));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok(await _userService.Logout(Request.Headers.Authorization.ToString().Replace("Bearer ", "")));

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("getProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _userService.GetProfile(name));

        }

        /*[HttpGet]
        public ActionResult<User> Get()
        {
            User user = new User();
            user.Id = new Guid();
            user.FullName = "Test";
            user.Email = "email@.ru";
            user.Password = "123";

            return user;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"This is GET with id = {id}";
        }

        [HttpDelete]
        public string Delete(int id)
        {
            return $"This is DELETE with id = {id}";
        }*/
    }
}
