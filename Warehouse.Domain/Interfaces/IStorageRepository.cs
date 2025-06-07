using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
