using ASP.NET.Models;
using ASP.NET.ModelsDTO.User;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASP.NET.Services
{
    public interface IUserService
    {
        Task<TokenResponse> Register(UserRegisterModel model);
        Task<TokenResponse> Login(LoginCredentials loginCredentials);
        Task<Response> Logout(string authorizationString);
        Task<UserDto> GetProfile(string authorizationString);
        Task<UserEditModel> EditProfile(string authorizationString);
    }

    public class UserService : IUserService
    {
        //обращение к БД
        private readonly TestContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _distributedCache;

        public UserService(TestContext context, UserManager<User> userManager, IDistributedCache distributedCache)
        {
            _context = context;
            _userManager = userManager;
            _distributedCache = distributedCache;
        }

        public async Task<TokenResponse> Register(UserRegisterModel model)
        {

            User user = new()
            {
                UserName = model.FullName,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,
                Gender = model.Gender,

            };
            //await _context.Users.AddAsync(user);
            

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                TokenResponse token = new TokenResponse();
                token.Token = result.Errors.First().Description;
                return token;
            }
            await _context.SaveChangesAsync();

            TokenResponse token2 = new TokenResponse();
            token2.Token = IHelper.GenerateToken(model.FullName);
            return token2;
        }

        public async Task<TokenResponse> Login(LoginCredentials loginCredentials)
        {

            var user = _context.Users.FirstOrDefault(x => x.Email == loginCredentials.Email);   //await _userManager.FindByNameAsync(loginCredentials.);
            var result = await _userManager.CheckPasswordAsync(user, loginCredentials.Password);
            TokenResponse token = new TokenResponse();
            if (result)
            {
                token.Token = IHelper.GenerateToken(user.UserName);
            }
            else
            {
                token.Token = "user does not exist";
            }
            return token;
        }

        public async Task<Response> Logout(string authorizationString)
        {

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(
                authorizationString
            );

            var key = $"denylist:{authorizationString}";

            var options = new DistributedCacheEntryOptions { AbsoluteExpiration = token.ValidTo };

            await _distributedCache.SetStringAsync(key, "invalid", options);

            Response response = new Response();

            return response;
        }

        public async Task<UserDto> GetProfile(string name)
        {
            var userFound = _context.Users.FirstOrDefault(a => a.UserName == name)!;
            UserDto user = new UserDto();
            user.Id = userFound.Id;
            user.PhoneNumber = userFound.PhoneNumber;
            user.Email = userFound.Email;
            user.BirthDate = userFound.BirthDate;
            user.FullName = userFound.UserName!;
            user.Gender = userFound.Gender;
            return user;
        }
        public async Task<UserEditModel> EditProfile(string name)
        {
            var userFound = _context.Users.FirstOrDefault(a => a.UserName == name)!;
            UserEditModel user = new UserEditModel();
            user.PhoneNumber = userFound.PhoneNumber;
            user.Email = userFound.Email;
            user.BirthDate = userFound.BirthDate;
            user.FullName = userFound.UserName!;
            user.Gender = userFound.Gender;
            return user;
        }
    }

}
