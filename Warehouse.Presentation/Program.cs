using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application.Mappings;
using Warehouse.Application.Service;
using Warehouse.Domain.Interfaces;
using Warehouse.Infrastructure.Models;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Infrastructure.Service;
using Warehouse.Presentation.UserCases;
class Program
{
    static void Main()
    {
        var services = new ServiceCollection();

        try
        {

            MappingConfig.Configure();
            services.AddDbContext<WarehouseDbContext>(options =>
            options.UseSqlite(DataProvider<ConfigModel>.GetConfigData().ConnectionString));
            services.AddScoped<IStorageRepository, StorageRepository>();
            services.AddScoped<IStorageService, WarehouseService>();
            services.AddSingleton<ConsoleMenu>();

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
                context.Database.EnsureCreated();
            }

            var menu = serviceProvider.GetRequiredService<ConsoleMenu>();

            menu.Show();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"При считывании конфигурационных данных БД произошла ошибка \n {ex.Message}\n");
            return;
        }
    }
}