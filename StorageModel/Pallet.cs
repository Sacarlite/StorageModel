using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Model
{
    public class Pallet : IStorageItem
    {
        private readonly List<Box> _boxes = new();

        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }

        public void AddBox(Box box)
        {
            if (box.Width > this.Width || box.Depth > this.Depth)
            {
                throw new InvalidOperationException("Коробка больше паллеты по размерам.");
            }
            _boxes.Add(box);
        }

        public DateTime ExpirationDate
        {
            get
            {
                if (!_boxes.Any())
                {
                    throw new InvalidOperationException("Паллета не содержит коробок.");
                }
                return _boxes.Min(b => b.ExpirationDate).Date;
            }
        }

        public double Weight => _boxes.Sum(b => b.Weight) + 30;

        public double Volume => Width * Height * Depth + _boxes.Sum(b => b.Volume);
    }
}
