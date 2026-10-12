using GhahveAlizade.Application.Features.Coffee.Dtos;
using GhahveAlizade.Domain.Coffee;
using Mapster;

namespace GhahveAlizade.Application.Features.Coffee;

public class CoffeeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // CoffeeOrigin
        config.NewConfig<CoffeeOrigin, CoffeeOriginDto>()
            .Map(dest => dest.RegionCount, src => src.Regions.Count(r => !r.IsDeleted))
            .Map(dest => dest.ProductCount, src => 0) // بعداً با Product محاسبه می‌شه
            .Map(dest => dest.Regions, src => src.Regions.Where(r => !r.IsDeleted).OrderBy(r => r.DisplayOrder).ToList());

        config.NewConfig<CoffeeOrigin, CoffeeOriginListItemDto>()
            .Map(dest => dest.RegionCount, src => src.Regions.Count(r => !r.IsDeleted))
            .Map(dest => dest.ProductCount, src => 0);

        config.NewConfig<CoffeeRegion, CoffeeRegionDto>();

        // CoffeeProcess
        config.NewConfig<CoffeeProcess, Dtos.CoffeeProcessDto>();
        config.NewConfig<CoffeeProcess, Dtos.CoffeeProcessListItemDto>();

        // RoastProfile
        config.NewConfig<RoastProfile, Dtos.RoastProfileDto>();
        config.NewConfig<RoastProfile, Dtos.RoastProfileListItemDto>();

        // FlavorNote
        config.NewConfig<FlavorNote, Dtos.FlavorNoteDto>()
            .Map(dest => dest.ProductCount, src => src.Products.Count(p => !p.IsDeleted));

        config.NewConfig<FlavorNote, Dtos.FlavorNoteListItemDto>()
            .Map(dest => dest.ProductCount, src => src.Products.Count(p => !p.IsDeleted));
    }
}