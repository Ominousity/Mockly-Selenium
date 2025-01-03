using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    
    public DbSet<Endpoint> Endpoints { get; set; }
    public DbSet<ResponseObject> ResponseObjects { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the Endpoint entity
        modelBuilder.Entity<Endpoint>(entity =>
        {
            entity.HasKey(e => e.Id); // Primary key

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100); // You can specify the max length

            entity.Property(e => e.Path)
                .IsRequired()
                .HasMaxLength(200); // Define path as required and limit its length

            entity.Property(e => e.Method)
                .IsRequired()
                .HasConversion<int>(); // Store enums as integers
        });

        modelBuilder.Entity<ResponseObject>()
            .Property(e => e.Data)
            .HasConversion(
                v => v, // Save as byte array directly
                v => v); // Load as byte array directly

        modelBuilder.Entity<ResponseObject>()
            .HasKey(e => e.Id);
    }
}