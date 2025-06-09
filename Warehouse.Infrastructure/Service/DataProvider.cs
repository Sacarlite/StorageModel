using Newtonsoft.Json;
using Warehouse.Domain.Interfaces;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Infrastructure.Service
{
    public static class DataProvider<T>
    {
        private static readonly Dictionary<Type, string> map = new()
        {
             { typeof(ConfigModel) ,  "config.json"},
         };
        private const string ResoursesPath = "\\Resources";

        public static T GetConfigData()
        {

            if (!map.TryGetValue(typeof(T), out var fileName))
            {
                throw new InvalidOperationException($"Нет зарегистрированных моделей для объекта {typeof(T)}");
            }

            var configPath = Path.Combine(PathService.GetCurentFolderPath(ResoursesPath), fileName);
            try
            {
                var serializedData = File.ReadAllText(configPath);
                var config = JsonConvert.DeserializeObject<T>(serializedData);

                if (config is null)
                {
                    throw new InvalidOperationException("Произошла ошибка при десериализации config файла");
                }

                if (config is IValidatable validatable)
                {
                    validatable.Validate();
                }

                return config;
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка при считывании и конвертации файла {fileName} {ex.Message}");
            }
        }
    }
}
