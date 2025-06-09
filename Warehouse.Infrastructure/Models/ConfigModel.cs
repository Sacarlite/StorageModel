using System.Runtime.Serialization;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Infrastructure.Models
{
    [DataContract]
    public class ConfigModel : IValidatable
    {
        [DataMember(Name = "connectionString")]
        public string ConnectionString { get; set; }

        [DataMember(Name = "maxPalletGenerationCount")]
        public int MaxPalletGenerationCount { get; set; }

        [DataMember(Name = "maxBoxGenerationCount")]
        public int MaxBoxGenerationCount { get; set; }

        [DataMember(Name = "basePalleteWeight")]
        public double BasePalleteWeight { get; set; }

        public void Validate()
        {
            var errors = new List<string>();


            if (MaxPalletGenerationCount < 0)
            {
                errors.Add("Количество паллет должно быть положительным.");
            }

            if (MaxBoxGenerationCount < 0)
            {
                errors.Add("Количество коробок на паллете должно быть положительным.");
            }

            if (BasePalleteWeight < 0)
            {
                errors.Add("Вес паллеты должен быть положительным.");
            }

            if (errors.Count > 0)
            {
                var errorText = string.Join(Environment.NewLine, errors);
                throw new InvalidOperationException($"Некорректная конфигурация:\n{errorText}");
            }
        }
    }
}
