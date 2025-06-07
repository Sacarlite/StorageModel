using Mapster;
using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Infrastructure
{
    public class StorageRepository : IStorageRepository
    {
        private readonly WarehouseDbContext _context;

        public StorageRepository(WarehouseDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Box> GetAllBoxes()
        {

            return _context.Boxes.AsNoTracking().Adapt<List<Box>>();
        }

        public IEnumerable<Pallet> GetAllPallets()
        {
            return _context.Pallets
                .AsNoTracking()
                .Include(p => p.Boxes)
                .Adapt<List<Pallet>>();
        }

        public void AddBox(Box box)
        {
            var dto = box.Adapt<BoxDTO>();
            _context.Boxes.Add(dto);
            _context.SaveChanges();
            box.Id = dto.Id;
        }

        public void AddPallet(Pallet pallet)
        {
            var dto = pallet.Adapt<PalletDTO>();
            _context.Pallets.Add(dto);
            _context.SaveChanges();
            pallet.Id = dto.Id;
        }
    }
}
