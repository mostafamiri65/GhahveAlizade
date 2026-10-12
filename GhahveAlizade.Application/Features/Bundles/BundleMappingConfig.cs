using GhahveAlizade.Application.Features.Bundles.Dtos;
using GhahveAlizade.Domain.Bundle;
using Mapster;

namespace GhahveAlizade.Application.Features.Bundles;

public class BundleMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<ProductBundle, ProductBundleDto>()
			.Map(dest => dest.Items, src => src.Items.Where(i => !i.IsDeleted).OrderBy(i => i.DisplayOrder).ToList());

		config.NewConfig<ProductBundle, ProductBundleListItemDto>()
			.Map(dest => dest.ItemCount, src => src.Items.Count(i => !i.IsDeleted));

		config.NewConfig<ProductBundleItem, ProductBundleItemDto>()
			.Map(dest => dest.ProductName, src => src.Product != null ? src.Product.Name : "")
			.Map(dest => dest.ProductImageUrl,
				src => src.Product != null
					? src.Product.Images.Where(i => !i.IsDeleted && i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()
					: null)
			.Map(dest => dest.VariantName, src => src.ProductVariant != null ? src.ProductVariant.Name : null);
	}
}