using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;
using Warehouse.Infrastructure.Service;

namespace Warehouse.Test.Step
{
    public static class WarehouseSteps
    {
        /// <summary>
        /// Создает паллету с заданными параметрами для тестов.
        /// </summary>
        /// <param name="id">Идентификатор паллеты.</param>
        /// <param name="expirationDate">Срок годности коробки на паллете (если есть).</param>
        /// <param name="weight">Общий вес паллеты (включая базовый вес 30 кг).</param>
        /// <param name="volume">Общий объем паллеты (без учета базового объема паллеты).</param>
        /// <returns>Объект <see cref="Pallet"/> с заданными параметрами.</returns>
        public static Pallet CreatePallet(int id, DateTime? expirationDate, double weight)
        {
            var pallet = new Pallet(DataProvider<ConfigModel>.GetConfigData().BasePalleteWeight)
            {
                Id = id,
                Width = id * 100,
                Height = id * 100,
                Depth = id * 100
            };
            if (expirationDate.HasValue)
            {
                pallet.Items.Add(new Box
                {
                    Id = id * 10,
                    Width = 10,
                    Height = 10,
                    Depth = 10,
                    Weight = weight - 30,
                    ExpirationDateOverride = expirationDate
                });
            }
            return pallet;
        }
    }
}
