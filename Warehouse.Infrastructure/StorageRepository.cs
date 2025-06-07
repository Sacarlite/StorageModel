using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Infrastructure
{
    public class StorageRepository : IStorageRepository
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;

        public StorageRepository(WarehouseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<Box> GetAllBoxes() =>
            _context.Boxes.ProjectTo<Box>(_mapper.ConfigurationProvider).ToList();

        public IEnumerable<Pallet> GetAllPallets() =>
            _context.Pallets.ProjectTo<Pallet>(_mapper.ConfigurationProvider).ToList();
        public void AddBox(Box box)
        {
            var dto = _mapper.Map<BoxDTO>(box);
            _context.Boxes.Add(dto);
            _context.SaveChanges();
        }

        public void AddPallet(Pallet pallet)
        {
            var dto = _mapper.Map<PalletDTO>(pallet);
            _context.Pallets.Add(dto);
            _context.SaveChanges();
        }
    }
}
