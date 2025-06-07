using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;

namespace Warehouse.Presentation
{
    public class ConsoleMenu
    {
        private readonly IStorageService _storageService;
        private readonly IStorageRepository _storageRepository;

        public ConsoleMenu(IStorageService storageService, IStorageRepository storageRepository)
        {
            _storageService = storageService;
            _storageRepository = storageRepository;
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
                    Console.WriteLine("Список паллет (сгруппированы по сроку годности, отсортированы по весу):");
                    foreach (var pallet in pallets)
                    {
                        Console.WriteLine(
                            $"ID: {pallet.Id}, Коробок: {pallet.Items.Count}, Срок: {pallet.ExpirationDate}, Вес: {pallet.Weight:F2} кг");
                    }

                    Console.WriteLine("\nТоп-3 паллеты с наибольшим сроком годности (отсортированы по объему):");
                    var topPallets = _storageService.GetTop3LongestLastingPallets().ToList();
                    foreach (var pallet in topPallets)
                    {
                        Console.WriteLine(
                            $"ID: {pallet.Id}, Коробок: {pallet.Items.Count}, Срок: {pallet.ExpirationDate}, Объем: {pallet.Volume:F2}");
                    }
                }

                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить новую паллету");
                Console.WriteLine("2. Выбрать паллету");
                Console.WriteLine("3. Выйти");

                Console.Write("\nВведите номер действия: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddPallet();
                        break;

                    case "2":
                        if (_storageRepository.GetAllPallets().Any())
                            ViewPalletMenu();
                        else
                            Console.WriteLine("Нет паллет для выбора.");
                        break;

                    case "3":
                        Console.WriteLine("Выход...");
                        return;

                    default:
                        Console.WriteLine("Неверный ввод.");
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
                Console.Write("Ширина: ");
                pallet.Width = double.Parse(Console.ReadLine());

                Console.Write("Высота: ");
                pallet.Height = double.Parse(Console.ReadLine());

                Console.Write("Глубина: ");
                pallet.Depth = double.Parse(Console.ReadLine());

                _storageRepository.AddPallet(pallet);
                Console.WriteLine($"Паллета добавлена с ID: {pallet.Id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void ViewPalletMenu()
        {
            var pallets = _storageRepository.GetAllPallets().ToList();
            Console.Clear();
            Console.WriteLine("___ Выберите паллету ___\n");

            for (int i = 0; i < pallets.Count; i++)
            {
                var pallet = pallets[i];
                Console.WriteLine(
                    $"{i + 1}. ID: {pallet.Id}, Коробок: {pallet.Items.Count}, Срок: {pallet.ExpirationDate}");
            }

            Console.Write("\nВведите номер паллеты: ");
            if (int.TryParse(Console.ReadLine(), out var index) && index > 0 && index <= pallets.Count)
            {
                var selectedPallet = pallets[index - 1];
                ShowPalletDetails(selectedPallet);
            }
            else
            {
                Console.WriteLine("Неверный номер.");
            }
        }

        private void ShowPalletDetails(Pallet pallet)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"___ Паллета {pallet.Id} ___\n");

                Console.WriteLine($"Коробок: {pallet.Items.Count}");
                Console.WriteLine($"Срок годности: {pallet.ExpirationDate}");
                Console.WriteLine($"Вес: {pallet.Weight:F2} кг");
                Console.WriteLine($"Объем: {pallet.Volume:F2}\n");

                if (!pallet.Items.Any())
                {
                    Console.WriteLine("Паллета пуста.");
                }
                else
                {
                    Console.WriteLine("Коробки на паллете:");
                    foreach (var box in pallet.Items.Cast<Box>())
                    {
                        Console.WriteLine(
                            $"ID: {box.Id}, Срок: {box.ExpirationDate}, Вес: {box.Weight:F2} кг");
                    }
                }

                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить коробку");
                Console.WriteLine("2. Назад");

                var choice = Console.ReadLine();
                if (choice == "1")
                    AddBoxToPallet(pallet);
                else if (choice == "2")
                    return;
                else
                    Console.WriteLine("Неверный выбор.");

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void AddBoxToPallet(Pallet pallet)
        {
            Console.Clear();
            Console.WriteLine("___ Добавление коробки ___");

            var box = new Box();
            try
            {
                Console.Write("Ширина: ");
                box.Width = double.Parse(Console.ReadLine());

                Console.Write("Высота: ");
                box.Height = double.Parse(Console.ReadLine());

                Console.Write("Глубина: ");
                box.Depth = double.Parse(Console.ReadLine());

                Console.Write("Вес: ");
                box.Weight = double.Parse(Console.ReadLine());

                Console.Write("Дата производства (введите дату или нажмите Enter для указания срока годности): ");
                var productionDateInput = Console.ReadLine();

                if (DateTime.TryParse(productionDateInput, out var productionDate))
                {
                    box.ProductionDate = productionDate;
                }
                else
                {
                    Console.Write("Срок годности: ");
                    var expirationDateInput = Console.ReadLine();
                    if (DateTime.TryParse(expirationDateInput, out var expirationDate))
                    {
                        box.ExpirationDateOverride = expirationDate;
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат даты.");
                        return;
                    }
                }

                pallet.AddItem(box);
                _storageRepository.AddBox(box);
                Console.WriteLine($"Коробка добавлена на паллету с ID: {box.Id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}