using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Presentation.Enums;
using Warehouse.Presentation.Service;

namespace Warehouse.Presentation.UserCases
{

    public class PalletMenu
    {
        private readonly IStorageRepository _storageRepository;

        public PalletMenu(IStorageRepository storageRepository)
        {
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
                ShowSelectedPalletMenu(selectedPallet);
                PrintSelectedPalletMenu();
                var choice = ValidationService.GetSelectedPalletMenuOption();

                switch (choice)
                {
                    case SelectedPalletMenuOption.Return:
                        continue;
                    case SelectedPalletMenuOption.Exit:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        return;
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
                    $"| {pallet.Id,-2} | {pallet.Items.Count,-7} | {pallet.ExpirationDate,-13} | {pallet.Weight,-8:F2} |");
            }
            Console.WriteLine("--------------------------------------------------");
        }

        private Pallet SelectPallet(List<Pallet> pallets)
        {
            while (true)
            {
                Console.Write("\nВведите номер паллеты: ");
                if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= pallets.Count)
                {
                    return pallets[index - 1];
                }
                Console.WriteLine("Неверный номер.Попробуйте снова");
            }
        }

        private void ShowSelectedPalletMenu(Pallet pallet)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"___ Паллета {pallet.Id} ___\n");

                Console.WriteLine($"Коробок: {pallet.Items.Count}");
                Console.WriteLine($"Срок годности: {pallet.ExpirationDate}");
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
                var choice = ValidationService.GetPalletMenuOption();

                switch (choice)
                {
                    case PalletMenuOption.AddBox:
                        AddBoxToPallet(pallet);
                        break;
                    case PalletMenuOption.Back:
                        return;
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
                    $"| {box.Id,-2} | {box.Width,-11:F2} | {box.Height,-11:F2} | {box.Depth,-12:F2} | {box.Weight,-8:F2} | {box.Volume,-11:F2} | {box.ExpirationDate,-13} |");
            }
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }

        private void PrintPalletMenu()
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine($"{(int)PalletMenuOption.AddBox}. Добавить коробку");
            Console.WriteLine($"{(int)PalletMenuOption.Back}. Назад");
        }

        private void PrintSelectedPalletMenu()
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine($"{(int)SelectedPalletMenuOption.Return}. Вернутся к выбору паллет");
            Console.WriteLine($"{(int)SelectedPalletMenuOption.Exit}. Выход");
        }

        private void AddBoxToPallet(Pallet pallet)
        {
            Console.Clear();
            Console.WriteLine("___ Добавление коробки ___");

            var box = new Box();
            try
            {
                box.Width = ValidationService.GetPositiveDouble("Ширина (см): ");
                box.Height = ValidationService.GetPositiveDouble("Высота (см): ");
                box.Depth = ValidationService.GetPositiveDouble("Глубина (см): ");
                box.Weight = ValidationService.GetPositiveDouble("Вес (кг): ");

                if (box.Width > pallet.Width || box.Depth > pallet.Depth)
                {
                    throw new ArgumentException("Коробка не помещается на паллету по ширине или глубине.");
                }

                ValidationService.ReadDate(box);
                pallet.AddItem(box);
                _storageRepository.AddBox(box);
                Console.WriteLine($"Коробка добавлена с ID: {box.Id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

    }
}

