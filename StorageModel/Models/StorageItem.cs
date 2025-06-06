using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageModel.Models
{
    public abstract class StorageItem
    {
        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public virtual double Weight { get; set; }
        public DateTime ExpirationDate => CalculateExpirationDate();
        public virtual double Volume => Width * Height * Depth;

        protected abstract DateTime CalculateExpirationDate();
    }
}
