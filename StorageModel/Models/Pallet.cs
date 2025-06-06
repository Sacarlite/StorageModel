using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageModel.Models
{
    public class Pallet : StorageItem
    {
        private List<Box> _boxes = new List<Box>();

        public void AddBox(Box box)
        {
            if (box.Width > this.Width || box.Depth > this.Depth)
            {
                throw new InvalidOperationException("Размеры коробки превышают размеры паллеты.");
            }

            _boxes.Add(box);
        }

        protected override DateTime CalculateExpirationDate()
        {
            if (!_boxes.Any())
                throw new InvalidOperationException("Паллета не содержит коробок.");

            return _boxes.Min(b => b.ExpirationDate).Date;
        }

        public override double Weight => _boxes.Sum(b => b.Weight) + 30;

        public override double Volume => base.Volume + _boxes.Sum(b => b.Volume);
    }
}
