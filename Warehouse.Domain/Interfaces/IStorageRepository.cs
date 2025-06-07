using Warehouse.Domain.Models;

namespace Warehouse.Domain.Interfaces
{
    public interface IStorageRepository
    {
        IEnumerable<Box> GetAllBoxes();
        IEnumerable<Pallet> GetAllPallets();
        void AddBox(Box box);
        void AddPallet(Pallet pallet);
    }
}
