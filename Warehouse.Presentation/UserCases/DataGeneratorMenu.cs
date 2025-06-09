using Warehouse.Domain.Interfaces;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;
using Warehouse.Infrastructure.Service;
using Warehouse.Presentation.Enums;
using Warehouse.Presentation.Service;

namespace Warehouse.Presentation.UserCases
{
    public class DataGeneratorMenu
    {
        private readonly IStorageRepository _storageRepository;
        private readonly TestDataGenerator _dataGenerator;
        private int _maxPalletGenerationCount;
        private int _maxBoxGenerationCount;
        public DataGeneratorMenu(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
            _dataGenerator = new TestDataGenerator();
        }

        public void ShowDataGeneratorMenu()
        {
            try
            {
                _maxPalletGenerationCount = DataProvider<ConfigModel>.GetConfigData().MaxPalletGenerationCount;
                _maxBoxGenerationCount = DataProvider<ConfigModel>.GetConfigData().MaxBoxGenerationCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"При считывании конфигурационных данных генератора произошла ошибка {ex.Message}\n");
                return;
            }

            var generatedPallets = new List<Pallet>();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("___ Генерация случайных данных ___\n");

                var palletCount = ValidationService.GetPositiveIntInRange($"Введите количество паллет (0-{_maxPalletGenerationCount}): ", 0, _maxPalletGenerationCount);
                var maxBoxesPerPallet = ValidationService.GetPositiveIntInRange($"Введите максимальное количество коробок на паллете (0-{_maxBoxGenerationCount}): ", 0, _maxBoxGenerationCount);

                generatedPallets = _dataGenerator.GeneratePallets(palletCount, maxBoxesPerPallet);
                Console.WriteLine("\nСгенерированные данные:");

                if (!generatedPallets.Any())
                {
                    Console.WriteLine("Данные не сгенерированы.");
                }
                else
                {
                    foreach (var pallet in generatedPallets)
                    {
                        PrintGeneratedPallet(pallet);
                    }
                }

                PrintDataGeneratorMenu();
                var choice = ValidationService.ReadDataGeneratorOption();

                switch (choice)
                {
                    case GenerateDataOption.SaveToDatabase:
                        SaveGeneratedData(generatedPallets);
                        return;
                    case GenerateDataOption.Regenerate:
                        continue;
                    case GenerateDataOption.Exit:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void PrintGeneratedPallet(Pallet pallet)
        {
            Console.WriteLine("\nПаллета:");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("| Ширина (см) | Высота (см) | Глубина (см) | Вес (кг) | Объем (см^3) | Коробок |");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine(
                $"| {pallet.Width,-11:F2} | {pallet.Height,-11:F2} | {pallet.Depth,-12:F2} | {pallet.Weight,-8:F2} | {pallet.Volume,-11:F2} | {pallet.Items.Count,-7} |");
            Console.WriteLine("-----------------------------------------------------------------------------------");

            if (pallet.Items.Any())
            {
                Console.WriteLine("\nКоробки на паллете:");
                Console.WriteLine("-----------------------------------------------------------------------------------");
                Console.WriteLine("| Ширина (см) | Высота (см) | Глубина (см) | Вес (кг) | Объем (см^3) | Срок годности |");
                Console.WriteLine("-----------------------------------------------------------------------------------");
                foreach (var box in pallet.Items.OfType<Box>())
                {
                    Console.WriteLine(
                        $"| {box.Width,-11:F2} | {box.Height,-11:F2} | {box.Depth,-12:F2} | {box.Weight,-8:F2} | {box.Volume,-11:F2} | {box.ExpirationDate,-13} |");
                }
                Console.WriteLine("-----------------------------------------------------------------------------------");
            }
        }

        private void PrintDataGeneratorMenu()
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine($"{(int)GenerateDataOption.SaveToDatabase}. Сохранить в базу данных");
            Console.WriteLine($"{(int)GenerateDataOption.Regenerate}. Повторить генерацию");
            Console.WriteLine($"{(int)GenerateDataOption.Exit}. Выйти");
        }

        private void SaveGeneratedData(List<Pallet> generatedPallets)
        {
            try
            {
                foreach (var pallet in generatedPallets)
                {
                    _storageRepository.AddPallet(pallet);
                }
                Console.WriteLine($"Сохранено {generatedPallets.Count} паллет в базу данных.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }
    }
}