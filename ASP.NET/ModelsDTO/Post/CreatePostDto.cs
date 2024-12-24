using ASP.NET.Models;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET.ModelsDTO.Post
{
    public class CreatePostDto
    {
        [MaxLength(1000), MinLength(5)]
        public string Title { get; set; }
        [MaxLength(5000), MinLength(5)]
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        public int ReadingTime { get; set; }
        [MaxLength(1000)]
        public string? Image { get; set; }
        public Guid AddressId { get; set; }
        [MinLength(1)]
        public List<Guid> Tags { get; set; }
    }
}
