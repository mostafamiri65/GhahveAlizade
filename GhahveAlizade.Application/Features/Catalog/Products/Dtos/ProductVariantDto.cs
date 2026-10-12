namespace GhahveAlizade.Application.Features.Catalog.Products.Dtos;

public class ProductVariantDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Sku { get; set; }
	public string? Barcode { get; set; }
	public string? AttributesJson { get; set; }

	public decimal Price { get; set; }
	public decimal? CompareAtPrice { get; set; }
	public decimal? CostPrice { get; set; }
	public decimal? Weight { get; set; }

	public string? ImageUrl { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; }
	public bool IsDefault { get; set; }
}