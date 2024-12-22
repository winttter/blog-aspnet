using ASP.NET.Models;

namespace ASP.NET.Mappers
{
    public static class TagMapper
    {
        public static TagDto ToDto(this Tag tag)
        {
            var tagDto = new TagDto
            {
                Id = tag.Id,
                CreateTime = tag.CreateTime,
                Name = tag.Name,
            };

            return tagDto;
        }

        public static List<TagDto> ToDtos(this List<Tag> tags)
        {
            return tags.Select(ToDto).ToList();
        }
    }
}
