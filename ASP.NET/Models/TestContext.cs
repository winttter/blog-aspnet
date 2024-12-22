using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace ASP.NET.Models
{
    public class TestContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Community>()
                .HasMany(g => g.Administrators)
                .WithMany(u => u.CommunityAdmin)
                .UsingEntity<Dictionary<string, object>>(
                    "CommunityAdmin",
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Community>().WithMany().HasForeignKey("CommunityId"),
                    j => j.ToTable("CommunityAdministrators")
                );

            modelBuilder.Entity<Community>()
                .HasMany(g => g.Subscribers)
                .WithMany(u => u.CommunitySubscriber)
                .UsingEntity<Dictionary<string, object>>(
                    "CommunitySubscriber",
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Community>().WithMany().HasForeignKey("CommunityId"),
                    j => j.ToTable("CommunitySubscribers")
                );

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Liker)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.LikerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.LikedPost)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.LikedPostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentPost)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ParentPostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public TestContext(DbContextOptions<TestContext> options): base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
