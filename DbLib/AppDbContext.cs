using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Domain.Entities;

namespace DbLib
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<TemplateFile> TemplateFiles { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Document>()
                .Property(d => d.Tags)
                .HasColumnType("jsonb");

            // List<string> Tags -> JSON
            modelBuilder.Entity<Document>()
                .Property(d => d.Tags)
                .HasColumnName("tags")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
                );

            // Уникальность избранного
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.DocumentId })
                .IsUnique();

            // Один документ <-> один шаблон
            modelBuilder.Entity<Document>()
                .HasOne(d => d.TemplateFile)
                .WithOne(tf => tf.Document)
                .HasForeignKey<TemplateFile>(tf => tf.DocumentId);
        }
    }
}
