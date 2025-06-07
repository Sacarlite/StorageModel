using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Models
{
    public class WarehouseDbContext : DbContext
    {
        public DbSet<BoxDTO> Boxes { get; set; }
        public DbSet<PalletDTO> Pallets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=warehouse.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoxDTO>().ToTable("Boxes");
            modelBuilder.Entity<PalletDTO>().ToTable("Pallets");
        }
    }
}
