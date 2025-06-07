using Warehouse.Domain.Interfaces;

namespace Warehouse.Domain.Models
{

    public class Box : IStorageItem, IHasWeight, IHasExpirationDate
    {
        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }
        public int? PalletId { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpirationDateOverride { get; set; }

        public ExpirationDateResult ExpirationDate
        {
            get
            {
                if (ExpirationDateOverride.HasValue)
                {
                    return ExpirationDateResult.FromDate(ExpirationDateOverride.Value);
                }
                if (ProductionDate.HasValue)
                {
                    return ExpirationDateResult.FromDate(ProductionDate.Value.AddDays(100));
                }
                throw new InvalidOperationException("Не указана дата производства или срок годности.");
            }
        }

        public double Volume => Width * Height * Depth;

    }

}
