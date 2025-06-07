using Warehouse.Domain.Models;

namespace Warehouse.Domain.Interfaces
{
    public interface IHasExpirationDate
    {
        ExpirationDateResult ExpirationDate { get; }
    }
}
