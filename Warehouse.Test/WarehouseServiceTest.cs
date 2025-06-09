using Moq;
using Warehouse.Application.Service;
using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Test.Step;

namespace Warehouse.Test
{
    public class WarehouseServiceTest
    {
        private Mock<IStorageRepository> _repositoryMock;
        private WarehouseService _warehouseService;

        /// <summary>
        /// Создает мок <see cref="IStorageRepository"/> и экземпляр <see cref="WarehouseService"/>.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IStorageRepository>();
            _warehouseService = new WarehouseService(_repositoryMock.Object);
        }

        /// <summary>
        /// Тест кейс проверки сортировки паллет по сроку годности, возрастанию даты и весу.
        /// </summary>
        [Test]
        public void GetSortedPalletsByExpiration_GroupsByExpirationDateAndSortsByDateThenWeight_ReturnsCorrectOrder()
        {
            var pallets = new List<Pallet>
            {
            WarehouseSteps.CreatePallet(1, new DateTime(2025, 1, 1), 50),
            WarehouseSteps.CreatePallet(2, new DateTime(2025, 1, 1), 60),
            WarehouseSteps.CreatePallet(3, new DateTime(2025, 2, 1), 70),
            WarehouseSteps.CreatePallet(4, null, 40)
            };
            _repositoryMock.Setup(r => r.GetAllPallets()).Returns(pallets);

            var result = _warehouseService.GetSortedPalletsByExpiration().ToList();

            Assert.AreEqual(4, result.Count, "Ожидалось 4 паллеты в результате.");
            Assert.AreEqual(1, result[0].Id, "Паллета с ID=1 должна быть второй (срок 2025-01-01, вес 50).");
            Assert.AreEqual(2, result[1].Id, "Паллета с ID=2 должна быть третьей (срок 2025-01-01, вес 60).");
            Assert.AreEqual(3, result[2].Id, "Паллета с ID=3 должна быть последней (срок 2025-02-01).");
            Assert.AreEqual(4, result[3].Id, "Паллета без срока годности должна быть последней.");
        }

        /// <summary>
        /// Тест кейс проверки возврата пустого списка
        /// </summary>
        [Test]
        public void GetSortedPalletsByExpiration_EmptyPalletList_ReturnsEmptyList()
        {
            _repositoryMock.Setup(r => r.GetAllPallets()).Returns(new List<Pallet>());

            var result = _warehouseService.GetSortedPalletsByExpiration().ToList();

            Assert.IsEmpty(result, "Ожидался пустой список для пустого набора паллет.");
        }

        /// <summary>
        /// Тест кейс проверки возврата паллет с наибольшим сроком годности, отсортированных по объему.
        /// </summary>
        [Test]
        public void GetTop3LongestLastingPallets_WithMultiplePallets_ReturnsTop3SortedByVolume()
        {
            var pallets = new List<Pallet>
            {
            WarehouseSteps.CreatePallet(1, new DateTime(2025, 1, 1), 50),
            WarehouseSteps.CreatePallet(2, new DateTime(2025, 2, 1), 60),
            WarehouseSteps.CreatePallet(3, new DateTime(2025, 3, 1), 70),
            WarehouseSteps.CreatePallet(4, new DateTime(2025, 4, 1), 40),
            WarehouseSteps.CreatePallet(5, new DateTime(2025, 5, 1), 20)
            };
            _repositoryMock.Setup(r => r.GetAllPallets()).Returns(pallets);

            var result = _warehouseService.GetTop3LongestLastingPallets().ToList();

            Assert.AreEqual(3, result.Count, "Ожидалось 3 паллеты в результате.");
            Assert.AreEqual(3, result[0].Id, "Паллета с ID=2 должна быть первой (объем 500).");
            Assert.AreEqual(4, result[1].Id, "Паллета с ID=1 должна быть второй (объем 1000).");
            Assert.AreEqual(5, result[2].Id, "Паллета с ID=3 должна быть третьей (объем 1500).");
        }

        /// <summary>
        /// Тест кейс проверки возврата паллет с наибольшим сроком годности, когда паллет меньше трех.
        /// </summary>
        [Test]
        public void GetTop3LongestLastingPallets_WithLessThanThreePallets_ReturnsAllSortedByVolume()
        {
            var pallets = new List<Pallet>
            {
            WarehouseSteps.CreatePallet(1, new DateTime(2025, 1, 1), 50),
            WarehouseSteps.CreatePallet(2, new DateTime(2025, 2, 1), 60)
            };
            _repositoryMock.Setup(r => r.GetAllPallets()).Returns(pallets);

            var result = _warehouseService.GetTop3LongestLastingPallets().ToList();

            Assert.AreEqual(2, result.Count, "Ожидалось 2 паллеты в результате.");
            Assert.AreEqual(2, result[0].Id, "Паллета с ID=2 должна быть первой (объем 500).");
            Assert.AreEqual(1, result[1].Id, "Паллета с ID=1 должна быть второй (объем 1000).");
        }


    }
}


