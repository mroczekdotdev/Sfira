using MarcinMroczek.Sfira.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace MarcinMroczek.Sfira.Data
{
    public class SfiraDbContext : IdentityDbContext<ApplicationUser>
    {
        public SfiraDbContext(DbContextOptions<SfiraDbContext> oupions) : base(oupions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.NormalizedEmail)
                .IsUnique();

            builder.Entity<UserPost>()
                .HasKey(up => new { up.UserId, up.PostId });

            builder.Entity<UserPost>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPosts)
                .HasForeignKey(up => up.UserId);

            builder.Entity<UserPost>()
                .HasOne(up => up.Post)
                .WithMany(p => p.UserPosts)
                .HasForeignKey(up => up.PostId);

            builder.Entity<UserPost>()
                .Property(e => e.Relation)
                .HasConversion(
                    v => v.ToString(),
                    v => (RelationType)Enum.Parse(typeof(RelationType), v));
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
