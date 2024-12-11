using Microsoft.EntityFrameworkCore;
namespace ASP.NET.Models
{
    public class TestContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public TestContext(DbContextOptions<TestContext> options): base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
