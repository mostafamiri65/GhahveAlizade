using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.CreateProduct;

public record CreateProductCommand(
	// اطلاعات اصلی
	string Name,
	string? Slug,
	string? ShortDescription,
	string? Description,
	string? Sku,
	string? Barcode,

	// قیمت
	bool HasVariants,
	decimal? BasePrice,
	decimal? CompareAtPrice,
	decimal? CostPrice,
	bool IsTaxable,
	decimal? TaxRate,

	// ابعاد
	decimal? Weight,
	decimal? Length,
	decimal? Width,
	decimal? Height,

	// روابط
	long? CategoryId,
	long? BrandId,
	long? CoffeeOriginId,
	long? CoffeeProcessId,
	long? RoastProfileId,
	List<long> FlavorNoteIds,

	// وضعیت
	bool IsActive,
	bool IsPublished,
	bool IsFeatured,
	bool IsNew,
	bool IsBestSeller,

	// SEO
	string? MetaTitle,
	string? MetaDescription,
	string? MetaKeywords,
	string? CanonicalUrl,
	string? OgImage,

	// مجموعه‌ها
	List<ProductVariantInput> Variants,
	List<ProductImageInput> Images,
	List<ProductAttributeInput> Attributes)
	: IRequest<ErrorOr<long>>;

public record ProductVariantInput(
	string Name,
	string? Sku,
	string? Barcode,
	string? AttributesJson,
	decimal Price,
	decimal? CompareAtPrice,
	decimal? CostPrice,
	decimal? Weight,
	string? ImageUrl,
	int DisplayOrder,
	bool IsActive,
	bool IsDefault);

public record ProductImageInput(
	string ImageUrl,
	string? ThumbnailUrl,
	string? AltText,
	string? Title,
	int DisplayOrder,
	bool IsPrimary,
	long? FileSizeBytes,
	int? Width,
	int? Height);

public record ProductAttributeInput(
	long AttributeId,
	long? AttributeValueId,
	string? CustomValue,
	int DisplayOrder);