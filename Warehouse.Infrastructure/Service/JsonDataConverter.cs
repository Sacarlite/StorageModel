
using Newtonsoft.Json;

namespace Warehouse.Infrastructure.Service
{
    public static class JsonDataConverter
    {
        public static T DeserializeData<T>(string json)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<T>(json);

                if (data is null)
                {
                    throw new InvalidOperationException("Произошла ошибка при десериализации json данных");
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка при конвертации json данных {ex.Message}");
            }
        }

        public static string SerializeData(object json)
        {
            try
            {
                var data = JsonConvert.SerializeObject(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка при сериализации объекта в json {ex.Message}");
            }
        }

    }
}
