using ASP.NET.Enums;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Comment;
using ASP.NET.ModelsDTO.Community;

namespace ASP.NET.Mappers
{
    public static class CommunityMapper
    {
        public static CommunityUserDto ToUserDto(this Community community, User user, CommunityRole role)
        {
            return new CommunityUserDto
            {
                UserId = user.Id,
                CommunityId = community.Id,
                Role = role
            };
        }

        public static CommunityFullDto ToFullDto(this Community community, int subscribersCount)
        {
            return new CommunityFullDto
            {
                Id = community.Id,
                CreateTime = community.CreateTime,
                Name = community.Name,
                Description = community.Description,
                IsClosed = community.IsClosed,
                SubscribersCount = subscribersCount,
                Administrators = community.Administrators.ToDtos()
            };
        }
    }
}
