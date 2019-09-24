using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MroczekDotDev.Sfira.Models;

namespace MroczekDotDev.Sfira.Data
{
    public class SfiraDbContext : IdentityDbContext<ApplicationUser>
    {
        public SfiraDbContext(DbContextOptions<SfiraDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var extensionEnumToStringConverter = new EnumToStringConverter<FilenameExtension>();

            builder.HasPostgresExtension("citext"); //PostgreSQL only

            builder.Entity<ApplicationUser>(e =>
            {
                e.HasIndex(u => u.NormalizedEmail).IsUnique();
                e.Property(u => u.UserName).HasColumnType("citext"); //PostgreSQL only //Should be fixed in 3.0
            });

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
                .Property(up => up.Relation)
                .HasConversion(new EnumToStringConverter<RelationType>());

            builder.Entity<Attachment>()
                .HasKey(a => a.Name);

            builder.Entity<ImageAttachment>()
                .HasBaseType<Attachment>()
                .Property(a => a.Extension)
                .HasConversion(extensionEnumToStringConverter);

            builder.Entity<Attachment>()
                .Property(a => a.Name)
                .ValueGeneratedNever();
        }
    }
}
