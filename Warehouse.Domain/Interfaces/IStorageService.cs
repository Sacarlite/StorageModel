using Warehouse.Domain.Models;

namespace Warehouse.Domain.Interfaces
{
    public interface IStorageService
    {
        IEnumerable<Pallet> GetSortedPalletsByExpiration();
        IEnumerable<Pallet> GetTop3LongestLastingPallets();
    }
}
