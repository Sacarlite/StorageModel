using Warehouse.Domain.Interfaces;


namespace Warehouse.Domain.Models
{
    public class Pallet : IStorageItem, IHasWeight, IHasExpirationDate, IContainer
    {
        private readonly double _baseWeight;

        public Pallet(double baseWeight = 30.0)
        {
            _baseWeight = baseWeight;
            Items = new List<Box>();
        }

        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public IList<Box> Items { get; set; }

        public void AddItem(IStorageItem item)
        {
            if (item is not Box box)
            {
                throw new ArgumentException("Палета может содержать только коробки.");
            }

            if (box.Width > Width || box.Depth > Depth)
            {
                throw new InvalidOperationException("Размеры коробки не должны превышать размеры палеты.");
            }

            box.PalletId = Id;
            Items.Add(box);
        }

        public ExpirationDateResult ExpirationDate
        {
            get
            {
                if (!Items.Any())
                {
                    return ExpirationDateResult.Empty;
                }

                return ExpirationDateResult.FromDate(Items.Cast<Box>().Min(b => b.ExpirationDate.Date!.Value));
            }
        }

        public double Weight => _baseWeight + Items.Cast<IHasWeight>().Sum(b => b.Weight);
        public double Volume => Width * Height * Depth + Items.Cast<Box>().Sum(b => b.Volume);
    }
}
