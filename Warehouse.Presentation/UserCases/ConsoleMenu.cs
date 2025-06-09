using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;
using Warehouse.Infrastructure.Service;
using Warehouse.Presentation.Enums;
using Warehouse.Presentation.Service;

namespace Warehouse.Presentation.UserCases
{
    public class ConsoleMenu
    {
        private readonly IStorageService _storageService;
        private readonly IStorageRepository _storageRepository;
        private readonly PalletMenu _palletMenu;
        private readonly DataGeneratorMenu _dataGeneratorMenu;

        public ConsoleMenu(IStorageService storageService, IStorageRepository storageRepository)
        {
            _storageService = storageService;
            _storageRepository = storageRepository;
            _palletMenu = new PalletMenu(storageRepository);
            _dataGeneratorMenu = new DataGeneratorMenu(storageRepository);
        }

        public void Show()
        {
            while (true)
            {
                var pallets = _storageService.GetSortedPalletsByExpiration().ToList();

                Console.Clear();
                Console.WriteLine("___ Склад ___\n");

                if (!pallets.Any())
                {
                    Console.WriteLine("Склад пуст. Нет паллет с коробками.");
                }
                else
                {
                    PrintPallets(pallets);
                    PrintTop3Pallets(pallets);
                }

                PrintMainMenu();
                var choice = ValidationService.ReadMainMenuOption();

                switch (choice)
                {
                    case MainMenuOption.AddPallet:
                        AddPallet();
                        break;
                    case MainMenuOption.SelectPallet:
                        if (_storageRepository.GetAllPallets().Any())
                        {
                            _palletMenu.ShowPalletMenu();
                        }
                        else
                        {
                            Console.WriteLine("Нет паллет для выбора.");
                        }
                        break;
                    case MainMenuOption.GenerateRandomData:
                        _dataGeneratorMenu.ShowDataGeneratorMenu();
                        break;
                    case MainMenuOption.Exit:
                        Console.WriteLine("Выход...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

            }
        }

        private void AddPallet()
        {
            Console.Clear();
            Console.WriteLine("___ Добавление паллеты ___");

            var pallet = new Pallet(DataProvider<ConfigModel>.GetConfigData().BasePalleteWeight);
            try
            {
                pallet.Width = ValidationService.GetPositiveDouble("Ширина (см): ");
                pallet.Height = ValidationService.GetPositiveDouble("Высота (см): ");
                pallet.Depth = ValidationService.GetPositiveDouble("Глубина (см): ");

                _storageRepository.AddPallet(pallet);
                Console.WriteLine($"Паллета добавлена с ID: {pallet.Id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void PrintPallets(List<Pallet> pallets)
        {
            Console.WriteLine("Список паллет (сгруппированы по сроку годности, отсортированы по весу):");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("| ID | Коробок | Срок годности | Ширина (см) | Глубина (см) | Вес (кг) | Объем (см^3) |");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            foreach (var pallet in pallets)
            {
                Console.WriteLine(
                    $"| {pallet.Id,-2} | {pallet.Items.Count,-7} | {pallet.ExpirationDate,-13} | {pallet.Width,-11:F2} | {pallet.Depth,-12:F2} | {pallet.Weight,-8:F2} | {pallet.Volume,-11:F2} |");
            }
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }

        private void PrintTop3Pallets(List<Pallet> pallets)
        {
            Console.WriteLine("\nТоп-3 паллеты с наибольшим сроком годности (отсортированы по объему):");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("| ID | Коробок | Срок годности | Ширина (см) | Глубина (см) | Вес (кг) | Объем (см³) |");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            var topPallets = _storageService.GetTop3LongestLastingPallets().ToList();
            foreach (var pallet in topPallets)
            {
                Console.WriteLine(
                    $"| {pallet.Id,-2} | {pallet.Items.Count,-7} | {pallet.ExpirationDate,-13} | {pallet.Width,-11:F2} | {pallet.Depth,-12:F2} | {pallet.Weight,-8:F2} | {pallet.Volume,-11:F2} |");
            }
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }

        private void PrintMainMenu()
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine($"{(int)MainMenuOption.AddPallet}. Добавить новую паллету");
            Console.WriteLine($"{(int)MainMenuOption.SelectPallet}. Выбрать паллету");
            Console.WriteLine($"{(int)MainMenuOption.GenerateRandomData}. Сгенерировать случайные данные");
            Console.WriteLine($"{(int)MainMenuOption.Exit}. Выйти");
        }

    }
}