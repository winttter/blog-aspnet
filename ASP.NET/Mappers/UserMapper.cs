using ASP.NET.Enums;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Author;
using ASP.NET.ModelsDTO.Community;
using ASP.NET.ModelsDTO.User;

namespace ASP.NET.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                CreateTime = user.CreateTime,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email!
            };
        }

        public static AuthorDto ToAuthorDto(this User user)
        {
            return new AuthorDto
            {
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Posts = user.Posts.Count(),
                Likes = user.Likes.Count(),
                Created = user.CreateTime
            };
        }

        public static List<UserDto> ToDtos(this List<User> users)
        {
            return users.Select(ToDto).ToList();
        }

        public static List<AuthorDto> ToAuthorDtos(this List<User> users)
        {
            return users.Select(ToAuthorDto).ToList();
        }
    }
}
