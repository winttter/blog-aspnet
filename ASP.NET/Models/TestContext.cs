using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace ASP.NET.Models
{
    public class TestContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Admin relationship
            modelBuilder.Entity<Community>()
                .HasMany(g => g.Administrators)
                .WithMany(u => u.CommunityAdmin)
                .UsingEntity<Dictionary<string, object>>(
                    "CommunityAdmin",
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Community>().WithMany().HasForeignKey("CommunityId"),
                    j => j.ToTable("CommunityAdministrators")
                );

            // Subscriber relationship
            modelBuilder.Entity<Community>()
                .HasMany(g => g.Subscribers)
                .WithMany(u => u.CommunitySubscriber)
                .UsingEntity<Dictionary<string, object>>(
                    "CommunitySubscriber",
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Community>().WithMany().HasForeignKey("CommunityId"),
                    j => j.ToTable("CommunitySubscribers")
                );
        }
        public TestContext(DbContextOptions<TestContext> options): base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
