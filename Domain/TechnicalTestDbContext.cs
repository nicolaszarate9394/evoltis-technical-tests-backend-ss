using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models.Entities;

namespace technical_tests_backend_ssr.Domain
{
    public class TechnicalTestDbContext : DbContext
    {
        public TechnicalTestDbContext(DbContextOptions<TechnicalTestDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");
            });
        }
    }
}
