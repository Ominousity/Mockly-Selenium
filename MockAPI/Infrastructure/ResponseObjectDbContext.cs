using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure
{
    public class ResponseObjectDbContext : DbContext
    {
        public ResponseObjectDbContext(DbContextOptions<ResponseObjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<ResponseObject> ResponseObjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configuration for byte[] Data property (no need for converter anymore)
            modelBuilder.Entity<ResponseObject>()
                .Property(e => e.Data)
                .HasConversion(
                    v => v, // Save as byte array directly
                    v => v); // Load as byte array directly

            modelBuilder.Entity<ResponseObject>()
                .HasKey(e => e.Id);
        }
    }
}
