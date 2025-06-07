using Mapster;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Application.Mappings
{
    public static class MappingConfig
    {
        public static void Configure()
        {

            TypeAdapterConfig<BoxDTO, Box>
                .NewConfig()
                .Map(dest => dest.ExpirationDateOverride, src => src.ExpirationDateOverride)
                .Map(dest => dest.ProductionDate, src => src.ProductionDate);

            TypeAdapterConfig<PalletDTO, Pallet>
                .NewConfig()
                .ConstructUsing(src => new Pallet(30.0))
                .Map(dest => dest.Width, src => src.Width)
                .Map(dest => dest.Height, src => src.Height)
                .Map(dest => dest.Depth, src => src.Depth)
                .Map(dest => dest.Items, src => src.Boxes)
                .AfterMapping((src, dest) =>
                {
                    foreach (var box in dest.Items.OfType<Box>())
                        box.PalletId = dest.Id;
                });

            TypeAdapterConfig<Pallet, PalletDTO>
                .NewConfig()
                .Map(dest => dest.Width, src => src.Width)
                .Map(dest => dest.Height, src => src.Height)
                .Map(dest => dest.Depth, src => src.Depth)
                .Map(dest => dest.Boxes, src => src.Items.OfType<Box>().Adapt<List<BoxDTO>>());
        }

    }

    public static class BoxMapper
    {
        public static ExpirationDateResult MapExpirationDate(BoxDTO src)
        {
            if (src.ExpirationDateOverride.HasValue)
                return ExpirationDateResult.FromDate(src.ExpirationDateOverride.Value);
            if (src.ProductionDate.HasValue)
                return ExpirationDateResult.FromDate(src.ProductionDate.Value.AddDays(100));
            return ExpirationDateResult.Empty;
        }
    }

}
