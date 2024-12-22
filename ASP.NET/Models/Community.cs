namespace ASP.NET.Models
{
    public class Community
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsClosed { get; set; }
        public List<User> Administrators { get; set; }
        public List<User> Subscribers { get; set; }
        public List<Post> Posts { get; set; }
    }
}
