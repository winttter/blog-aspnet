﻿using ASP.NET.Helper;
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
        Task EditProfile(string authorizationString, UserEditModel model);
    }

    public class UserService : IUserService
    {
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
            if (model.BirthDate > DateTime.UtcNow)
            {
                throw new Exception("400*you are not born yet");
            }

            User user = new()
            {
                UserName = model.FullName,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,
                Gender = model.Gender,

            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception("400*" + result.Errors.First().Description);
            }
            await _context.SaveChangesAsync();

            TokenResponse token2 = new TokenResponse();
            token2.Token = IHelper.GenerateToken(model.FullName);
            return token2;
        }

        public async Task<TokenResponse> Login(LoginCredentials loginCredentials)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == loginCredentials.Email);   
            var result = await _userManager.CheckPasswordAsync(user, loginCredentials.Password);
            TokenResponse token = new TokenResponse();
            if (result)
            {
                token.Token = IHelper.GenerateToken(user.UserName);
            }
            else
            {
                throw new Exception("404*user does not exist");
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
        public async Task EditProfile(string name, UserEditModel model)
        {
            if (model.BirthDate > DateTime.UtcNow)
            {
                throw new Exception("400*you are not born yet");
            }

            var userFound = _context.Users.FirstOrDefault(a => a.UserName == name)!;
            userFound.PhoneNumber = model.PhoneNumber;
            userFound.Email = model.Email;
            userFound.BirthDate = model.BirthDate;
            userFound.UserName = model.FullName;
            userFound.Gender = model.Gender;

            await _context.SaveChangesAsync();
        }
    }

}
