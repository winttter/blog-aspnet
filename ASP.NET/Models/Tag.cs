namespace ASP.NET.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
    }
}
