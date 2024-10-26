using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            // Configure the value converter for Dictionary to byte[] conversion
            var converter = new ValueConverter<Dictionary<string, ObjectType>, byte[]>(
                v => ResponseObject.SerializeHeaders(v),
                v => ResponseObject.DeserializeHeaders(v));

            var comparer = new ValueComparer<Dictionary<string, ObjectType>>(
            (d1, d2) => JsonSerializer.Serialize(d1, (JsonSerializerOptions)null) == JsonSerializer.Serialize(d2, (JsonSerializerOptions)null),
                d => JsonSerializer.Serialize(d, (JsonSerializerOptions)null).GetHashCode(),
                d => ResponseObject.DeserializeHeaders(ResponseObject.SerializeHeaders(d)));

            modelBuilder.Entity<ResponseObject>()
                .Property(e => e.Data)
                .HasConversion(converter)
                .Metadata.SetValueComparer(comparer);

            base.OnModelCreating(modelBuilder);
        }
    }
}
