using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Domain.Interfaces
{
    public interface IStorageItem
    {
        int Id { get; }
        double Width { get; }
        double Height { get; }
        double Depth { get; }
        double Weight { get; }
        DateTime ExpirationDate { get; }
        double Volume { get; }
    }
}
