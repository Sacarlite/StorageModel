using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Service
{
    public interface IWarehouseService
    {
        IEnumerable<Pallet> GetSortedPalletsByExpiration();
        IEnumerable<Pallet> GetTop3LongestLastingPallets();
    }
}
