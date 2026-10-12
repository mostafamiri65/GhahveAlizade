using GhahveAlizade.Application.Features.Catalog.Brands.Dtos;
using GhahveAlizade.Domain.Catalog;
using Mapster;

namespace GhahveAlizade.Application.Features.Catalog.Brands;

public class BrandMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Brand, BrandDto>()
            .Map(dest => dest.ProductCount,
                src => src.Products.Count(p => !p.IsDeleted));

        config.NewConfig<Brand, BrandListItemDto>()
            .Map(dest => dest.ProductCount,
                src => src.Products.Count(p => !p.IsDeleted));
    }
}