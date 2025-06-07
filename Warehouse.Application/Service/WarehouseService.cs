using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Service
{
    public class WarehouseService : IStorageService
    {
        private readonly IStorageRepository _repository;

        public WarehouseService(IStorageRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Pallet> GetSortedPalletsByExpiration()
        {
            return _repository.GetAllPallets()
                .GroupBy(p => p.ExpirationDate.HasValue ? p.ExpirationDate.Date : null)
                .OrderBy(g => g.Key ?? DateTime.MaxValue)
                .SelectMany(g => g.OrderBy(p => p.Weight));
        }

        public IEnumerable<Pallet> GetTop3LongestLastingPallets()
        {
            return _repository.GetAllPallets()
                .Where(p => p.Items.Any())
                .OrderByDescending(p => p.ExpirationDate.Date)
                .Take(3)
                .OrderBy(p => p.Volume);
        }
    }

}
