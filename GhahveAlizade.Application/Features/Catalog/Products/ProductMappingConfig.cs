using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using GhahveAlizade.Domain.Catalog;
using Mapster;

namespace GhahveAlizade.Application.Features.Catalog.Products;

public class ProductMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		// Product → ProductListItemDto
		config.NewConfig<Product, ProductListItemDto>()
			.Map(dest => dest.CategoryName,
				src => src.Category != null ? src.Category.Name : null)
			.Map(dest => dest.BrandName,
				src => src.Brand != null ? src.Brand.Name : null)
			.Map(dest => dest.CoffeeOriginName,
				src => src.CoffeeOrigin != null ? src.CoffeeOrigin.Name : null)
			.Map(dest => dest.RoastProfileName,
				src => src.RoastProfile != null ? src.RoastProfile.Name : null)
			.Map(dest => dest.CoffeeProcessName,
				src => src.CoffeeProcess != null ? src.CoffeeProcess.Name : null)
			.Map(dest => dest.PrimaryImageUrl,
				src => src.Images
					.Where(i => !i.IsDeleted && i.IsPrimary)
					.Select(i => i.ImageUrl)
					.FirstOrDefault()
					?? src.Images
						.Where(i => !i.IsDeleted)
						.OrderBy(i => i.DisplayOrder)
						.Select(i => i.ImageUrl)
						.FirstOrDefault())
			.Map(dest => dest.MinVariantPrice,
				src => src.Variants
					.Where(v => !v.IsDeleted && v.IsActive)
					.Min(v => (decimal?)v.Price))
			.Map(dest => dest.MaxVariantPrice,
				src => src.Variants
					.Where(v => !v.IsDeleted && v.IsActive)
					.Max(v => (decimal?)v.Price))
			.Map(dest => dest.TotalStock, src => 0); // بعداً با Inventory محاسبه می‌شه

		// Product → ProductDetailDto
		config.NewConfig<Product, ProductDetailDto>()
			.Map(dest => dest.CategoryName,
				src => src.Category != null ? src.Category.Name : null)
			.Map(dest => dest.BrandName,
				src => src.Brand != null ? src.Brand.Name : null)
			.Map(dest => dest.CoffeeOriginName,
				src => src.CoffeeOrigin != null ? src.CoffeeOrigin.Name : null)
			.Map(dest => dest.CoffeeProcessName,
				src => src.CoffeeProcess != null ? src.CoffeeProcess.Name : null)
			.Map(dest => dest.RoastProfileName,
				src => src.RoastProfile != null ? src.RoastProfile.Name : null)
			.Map(dest => dest.FlavorNoteIds,
				src => src.FlavorNotes.Where(f => !f.IsDeleted).Select(f => f.Id).ToList())
			.Map(dest => dest.FlavorNoteNames,
				src => src.FlavorNotes.Where(f => !f.IsDeleted).Select(f => f.Name).ToList())
			.Map(dest => dest.Variants,
				src => src.Variants.Where(v => !v.IsDeleted).OrderBy(v => v.DisplayOrder).ToList())
			.Map(dest => dest.Images,
				src => src.Images.Where(i => !i.IsDeleted).OrderBy(i => i.DisplayOrder).ToList())
			.Map(dest => dest.Attributes,
				src => src.Attributes.Where(a => !a.IsDeleted).OrderBy(a => a.DisplayOrder).ToList());

		// ProductVariant → ProductVariantDto
		config.NewConfig<ProductVariant, ProductVariantDto>();

		// ProductImage → ProductImageDto
		config.NewConfig<ProductImage, ProductImageDto>();

		// ProductAttribute → ProductAttributeDto
		config.NewConfig<ProductAttribute, ProductAttributeDto>()
			.Map(dest => dest.AttributeName,
				src => src.Attribute != null ? src.Attribute.Name : string.Empty)
			.Map(dest => dest.AttributeSlug,
				src => src.Attribute != null ? src.Attribute.Slug : string.Empty)
			.Map(dest => dest.Unit,
				src => src.Attribute != null ? src.Attribute.Unit : null)
			.Map(dest => dest.Value,
				src => src.AttributeValue != null
					? (src.AttributeValue.DisplayValue ?? src.AttributeValue.Value)
					: src.CustomValue);
	}
}