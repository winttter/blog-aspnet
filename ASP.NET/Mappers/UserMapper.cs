using ASP.NET.Enums;
using ASP.NET.Models;
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

        public static List<UserDto> ToDtos(this List<User> users)
        {
            return users.Select(ToDto).ToList();
        }
    }
}
