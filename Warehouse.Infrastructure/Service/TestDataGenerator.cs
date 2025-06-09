using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Infrastructure.Service
{
    public class TestDataGenerator
    {
        private Random _random;
        public TestDataGenerator()
        {
            _random = new Random();
        }

        public List<Pallet> GeneratePallets(int palletCount, int maxBoxesPerPallet)
        {
            if (palletCount < 0) throw new ArgumentException("Количество паллет не может быть отрицательным.", nameof(palletCount));
            if (maxBoxesPerPallet < 0) throw new ArgumentException("Количество коробок не может быть отрицательным.", nameof(maxBoxesPerPallet));

            var pallets = new List<Pallet>();
            for (int i = 0; i < palletCount; i++)
            {
                var pallet = new Pallet(DataProvider<ConfigModel>.GetConfigData().BasePalleteWeight)
                {
                    Width = 80 + _random.NextDouble() * 40,
                    Height = 80 + _random.NextDouble() * 40,
                    Depth = 80 + _random.NextDouble() * 40,
                    Items = new List<Box>()
                };

                int boxCount = _random.Next(0, maxBoxesPerPallet + 1);
                for (int j = 0; j < boxCount; j++)
                {
                    var box = new Box
                    {
                        Width = 10 + _random.NextDouble() * (pallet.Width - 10),
                        Height = 10 + _random.NextDouble() * 40,
                        Depth = 10 + _random.NextDouble() * (pallet.Depth - 10),
                        Weight = 1 + _random.NextDouble() * 29,
                        ProductionDate = DateTime.Today.AddDays(-_random.Next(0, 365))
                    };
                    pallet.AddItem(box);
                }
                pallets.Add(pallet);
            }
            return pallets;
        }
    }

}
