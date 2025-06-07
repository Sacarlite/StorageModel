using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Presentation.Enums;

namespace Warehouse.Presentation.UserCases
{


    public class PalletMenu
    {
        private readonly IStorageService _storageService;
        private readonly IStorageRepository _storageRepository;

        public PalletMenu(IStorageService storageService, IStorageRepository storageRepository)
        {
            _storageService = storageService;
            _storageRepository = storageRepository;
        }

        public void ShowPalletMenu()
        {
            var pallets = _storageRepository.GetAllPallets().ToList();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("___ Выберите паллету ___\n");

                if (!pallets.Any())
                {
                    Console.WriteLine("Нет паллет для выбора.");
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    return;
                }

                PrintPalletList(pallets);
                var selectedPallet = SelectPallet(pallets);

                if (selectedPallet != null)
                {
                    ShowSelectedPalletMenu(selectedPallet);
                }
            }
        }

        private void PrintPalletList(List<Pallet> pallets)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("| ID | Коробок | Срок годности | Вес (кг) |");
            Console.WriteLine("--------------------------------------------------");
            for (int i = 0; i < pallets.Count; i++)
            {
                var pallet = pallets[i];
                Console.WriteLine(
                    $"| {pallet.Id,-2} | {pallet.Items.Count,-7} | {pallet.ExpirationDate.Date?.ToString("yyyy-MM-dd"),-13} | {pallet.Weight,-8:F2} |");
            }
            Console.WriteLine("--------------------------------------------------");
        }

        private Pallet SelectPallet(List<Pallet> pallets)
        {
            Console.Write("\nВведите номер паллеты: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= pallets.Count)
            {
                return pallets[index - 1];
            }
            Console.WriteLine("Неверный номер.");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return null;
        }

        private void ShowSelectedPalletMenu(Pallet pallet)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"___ Паллета {pallet.Id} ___\n");

                Console.WriteLine($"Коробок: {pallet.Items.Count}");
                Console.WriteLine($"Срок годности: {pallet.ExpirationDate.Date?.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Вес: {pallet.Weight:F2} кг");
                Console.WriteLine($"Объем: {pallet.Volume:F2} см^3");
                Console.WriteLine($"Размеры: {pallet.Width:F2}x{pallet.Height:F2}x{pallet.Depth:F2} см\n");

                if (!pallet.Items.Any())
                {
                    Console.WriteLine("Паллета пустая.");
                }
                else
                {
                    PrintBoxes(pallet.Items.OfType<Box>().ToList());
                }

                PrintPalletMenu();
                var choice = ReadPalletMenuOption();

                switch (choice)
                {
                    case PalletMenuOption.AddBox:
                        AddBoxToPallet(pallet);
                        break;
                    case PalletMenuOption.Back:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void PrintBoxes(List<Box> boxes)
        {
            Console.WriteLine("Коробки на паллете:");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("| ID | Ширина (см) | Высота (см) | Глубина (см) | Вес (кг) | Объем (см^3) | Срок годности |");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            foreach (var box in boxes)
            {
                Console.WriteLine(
                    $"| {box.Id,-2} | {box.Width,-11:F2} | {box.Height,-11:F2} | {box.Depth,-12:F2} | {box.Weight,-8:F2} | {box.Volume,-11:F2} | {box.ExpirationDate.Date?.ToString("yyyy-MM-dd"),-13} |");
            }
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }

        private void PrintPalletMenu()
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine($"{(int)PalletMenuOption.AddBox}. Добавить коробку");
            Console.WriteLine($"{(int)PalletMenuOption.Back}. Назад");
        }

        private PalletMenuOption ReadPalletMenuOption()
        {
            Console.Write("\nВведите номер действия: ");
            if (int.TryParse(Console.ReadLine(), out int input) && Enum.IsDefined(typeof(PalletMenuOption), input))
            {
                return (PalletMenuOption)input;
            }
            return PalletMenuOption.None;
        }

        private void AddBoxToPallet(Pallet pallet)
        {
            Console.Clear();
            Console.WriteLine("___ Добавление коробки ___");

            var box = new Box();
            try
            {
                box.Width = ReadPositiveDouble("Ширина (см): ");
                box.Height = ReadPositiveDouble("Высота (см): ");
                box.Depth = ReadPositiveDouble("Глубина (см): ");
                box.Weight = ReadPositiveDouble("Вес (кг): ");

                if (box.Width > pallet.Width || box.Depth > pallet.Depth)
                {
                    throw new ArgumentException("Коробка не помещается на паллету по ширине или глубине.");
                }

                ReadDate(box);
                pallet.AddItem(box);
                _storageRepository.AddBox(box);
                Console.WriteLine($"Коробка добавлена с ID: {box.Id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void ReadDate(Box box)
        {
            Console.Write("Дата производства (гггг-мм-дд, или Enter для срока годности): ");
            var productionDateInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(productionDateInput))
            {
                if (DateTime.TryParse(productionDateInput, out var productionDate))
                {
                    box.ProductionDate = productionDate;
                }
                else
                {
                    throw new ArgumentException("Неверный формат даты производства.");
                }
            }
            else
            {
                Console.Write("Срок годности (гггг-мм-дд): ");
                var expirationDateInput = Console.ReadLine();
                if (DateTime.TryParse(expirationDateInput, out var expirationDate))
                {
                    box.ExpirationDateOverride = expirationDate;
                }
                else
                {
                    throw new ArgumentException("Неверный формат срока годности.");
                }
            }
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

