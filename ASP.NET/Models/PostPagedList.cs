namespace ASP.NET.Models
{
    public class PostPagedList
    {
        public List<PostDto> Posts { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
