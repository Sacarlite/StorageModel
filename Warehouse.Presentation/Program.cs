using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application.Mappings;
using Warehouse.Application.Service;
using Warehouse.Domain.Interfaces;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Models;
using Warehouse.Presentation;
class Program
{
    static void Main()
    {
        var services = new ServiceCollection();

        MappingConfig.Configure();

        services.AddDbContext<WarehouseDbContext>(options =>
            options.UseSqlite("Data Source=warehouse.db"));

        services.AddScoped<IStorageRepository, StorageRepository>();
        services.AddScoped<IStorageService, WarehouseService>();
        services.AddSingleton<ConsoleMenu>();

        var serviceProvider = services.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
            //   context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        var menu = serviceProvider.GetRequiredService<ConsoleMenu>();

        menu.Show();
    }
}