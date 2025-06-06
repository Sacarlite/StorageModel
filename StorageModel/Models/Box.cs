using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageModel.Models
{
    public class Box : StorageItem
    {
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpirationDateOverride { get; set; }

        protected override DateTime CalculateExpirationDate()
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
}
