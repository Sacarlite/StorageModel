using Microsoft.EntityFrameworkCore;

namespace Warehouse.Infrastructure.Models
{
    public class WarehouseDbContext : DbContext
    {
        public DbSet<BoxDTO> Boxes { get; set; }
        public DbSet<PalletDTO> Pallets { get; set; }

        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BoxDTO>(entity =>
            {
                entity.ToTable("Boxes");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Width).IsRequired();
                entity.Property(b => b.Height).IsRequired();
                entity.Property(b => b.Depth).IsRequired();
                entity.Property(b => b.Weight).IsRequired();
                entity.Property(b => b.PalletId).HasColumnName("PalletId");
                entity.Property(b => b.ProductionDate).HasColumnName("ProductionDate");
                entity.Property(b => b.ExpirationDateOverride).HasColumnName("ExpirationDate");
            });

            modelBuilder.Entity<PalletDTO>(entity =>
            {
                entity.ToTable("Pallets");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Width).IsRequired();
                entity.Property(p => p.Height).IsRequired();
                entity.Property(p => p.Depth).IsRequired();
                entity.HasMany(p => p.Boxes)
                      .WithOne()
                      .HasForeignKey("PalletId")
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}