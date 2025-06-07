using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Model
{
    public class Box : IStorageItem
    {
        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }

        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpirationDateOverride { get; set; }

        public DateTime ExpirationDate
        {
            get
            {
                if (ExpirationDateOverride.HasValue)
                {
                    return ExpirationDateOverride.Value.Date;
                }
                if (ProductionDate.HasValue)
                {
                    return ProductionDate.Value.Date.AddDays(100);
                }
                throw new InvalidOperationException("Не указана дата производства или срок годности.");
            }
        }

        public double Volume => Width * Height * Depth;

    }
}
