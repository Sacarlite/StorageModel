using Warehouse.Domain.Models;
using Warehouse.Presentation.Enums;

namespace Warehouse.Presentation.Service
{
    public static class ValidationService
    {
        public static double GetPositiveDouble(string prompt)
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

        public static int GetPositiveIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Введите число от {min} до {max}.");
            }
        }
        public static void ReadDate(Box box)
        {
            while (true)
            {
                Console.Write("Дата производства (гггг-мм-дд, или Enter для срока годности): ");
                var productionDateInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(productionDateInput))
                {
                    if (DateTime.TryParse(productionDateInput, out var productionDate))
                    {
                        box.ProductionDate = productionDate;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Неверный формат даты. Попробуйте снова.");
                        continue;
                    }
                }
                else
                {
                    while (true)
                    {
                        Console.Write("Срок годности (гггг-мм-дд): ");
                        var expirationDateInput = Console.ReadLine();

                        if (DateTime.TryParse(expirationDateInput, out var expirationDate))
                        {
                            box.ExpirationDateOverride = expirationDate;
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: Неверный формат даты. Попробуйте снова.");
                        }
                    }
                }
            }
        }

        public static MainMenuOption ReadMainMenuOption()
        {
            Console.Write("\nВведите номер действия: ");
            if (int.TryParse(Console.ReadLine(), out int input) && Enum.IsDefined(typeof(MainMenuOption), input))
            {
                return (MainMenuOption)input;
            }
            return MainMenuOption.None;
        }

        public static GenerateDataOption ReadDataGeneratorOption()
        {
            Console.Write("\nВведите номер действия: ");
            if (int.TryParse(Console.ReadLine(), out int input) && Enum.IsDefined(typeof(GenerateDataOption), input))
            {
                return (GenerateDataOption)input;
            }
            return GenerateDataOption.None;
        }
        public static PalletMenuOption GetPalletMenuOption()
        {
            Console.Write("\nВведите номер действия: ");
            if (int.TryParse(Console.ReadLine(), out int input) && Enum.IsDefined(typeof(PalletMenuOption), input))
            {
                return (PalletMenuOption)input;
            }
            return PalletMenuOption.None;
        }
        public static SelectedPalletMenuOption GetSelectedPalletMenuOption()
        {
            Console.Write("\nВведите номер действия: ");
            if (int.TryParse(Console.ReadLine(), out int input) && Enum.IsDefined(typeof(SelectedPalletMenuOption), input))
            {
                return (SelectedPalletMenuOption)input;
            }
            return SelectedPalletMenuOption.None;
        }
    }
}
