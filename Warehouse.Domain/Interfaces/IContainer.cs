using Warehouse.Domain.Models;

namespace Warehouse.Domain.Interfaces
{
    public interface IContainer
    {
        IList<Box> Items { get; set; }
        void AddItem(IStorageItem item);
    }
}
