using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP.NET.Models;
using ASP.NET.Services;

namespace ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserService _userService;
        public TestController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            
            return Ok(await _userService.Register(model));
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
