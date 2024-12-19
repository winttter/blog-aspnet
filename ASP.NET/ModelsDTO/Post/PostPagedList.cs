using ASP.NET.Models;

namespace ASP.NET.ModelsDTO.Post
{
    public class PostPagedList
    {
        public List<PostDto> Posts { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
