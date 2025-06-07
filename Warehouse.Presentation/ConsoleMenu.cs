using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Presentation.Enums;
using Warehouse.Presentation.UserCases;

namespace Warehouse.Presentation
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
            _palletMenu = new PalletMenu(storageService, storageRepository);
            _dataGeneratorMenu = new DataGeneratorMenu(storageService, storageRepository);
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
                var choice = ReadMainMenuOption();

                switch (choice)
                {
                    case MainMenuOption.AddPallet:
                        AddPallet();
                        break;
                    case MainMenuOption.SelectPallet:
                        if (_storageRepository.GetAllPallets().Any())
                            _palletMenu.ShowPalletMenu();
                        else
                            Console.WriteLine("Нет паллет для выбора.");
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

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void AddPallet()
        {
            Console.Clear();
            Console.WriteLine("___ Добавление паллеты ___");

            var pallet = new Pallet();
            try
            {
                pallet.Width = ReadPositiveDouble("Ширина (см): ");
                pallet.Height = ReadPositiveDouble("Высота (см): ");
                pallet.Depth = ReadPositiveDouble("Глубина (см): ");

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
                    $"| {pallet.Id,-2} | {pallet.Items.Count,-7} | {pallet.ExpirationDate.Date?.ToString("yyyy-MM-dd"),-13} | {pallet.Width,-11:F2} | {pallet.Depth,-12:F2} | {pallet.Weight,-8:F2} | {pallet.Volume,-11:F2} |");
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
                    $"| {pallet.Id,-2} | {pallet.Items.Count,-7} | {pallet.ExpirationDate.Date?.ToString("yyyy-MM-dd"),-13} | {pallet.Width,-11:F2} | {pallet.Depth,-12:F2} | {pallet.Weight,-8:F2} | {pallet.Volume,-11:F2} |");
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

        private MainMenuOption ReadMainMenuOption()
        {
            Console.Write("\nВведите номер действия: ");
            if (int.TryParse(Console.ReadLine(), out int input) && Enum.IsDefined(typeof(MainMenuOption), input))
            {
                return (MainMenuOption)input;
            }
            return MainMenuOption.None;
        }

        private double ReadPositiveDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double value) && value > 0)
                {
                    return value;
                }
                Console.WriteLine("Введите положительное число.");
            }
        }

    }
}