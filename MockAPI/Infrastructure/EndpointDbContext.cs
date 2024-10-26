using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EndpointDbContext : DbContext
    {
        public EndpointDbContext(DbContextOptions<EndpointDbContext> options)
            : base(options)
        {
        }

        public DbSet<Endpoint> Endpoints { get; set; }

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
        }
    }
}
