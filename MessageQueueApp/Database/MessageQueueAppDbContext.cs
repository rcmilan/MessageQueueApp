using MessageQueueApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageQueueApp.Database
{
    public class MessageQueueAppDbContext : DbContext
    {
        public MessageQueueAppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserMessage> UserMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserMessage>((entityTypeBuilder) => {
                entityTypeBuilder.HasKey(m => m.Id);
                entityTypeBuilder.Property(m => m.CreatedAt).IsRequired();
                entityTypeBuilder.Property(m => m.Content).IsRequired();
            });
        }
    }
}
