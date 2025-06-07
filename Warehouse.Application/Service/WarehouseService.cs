using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Application.Service
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IStorageRepository _repository;

        public WarehouseService(IStorageRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Pallet> GetSortedPalletsByExpiration()
        {
            return _repository.GetAllPallets()
                .GroupBy(p => p.ExpirationDate)
                .OrderBy(g => g.Key)
                .SelectMany(g => g.OrderBy(p => p.Weight)); 
        }

        public IEnumerable<Pallet> GetTop3LongestLastingPallets()
        {
            return _repository.GetAllPallets()
                .Where(p => p.Boxes.Count > 0) 
                .OrderByDescending(p => p.ExpirationDate) 
                .Take(3) 
                .OrderBy(p => p.Volume);
        }
    }
}
