namespace GhahveAlizade.Application.Features.Catalog.Products.Dtos;

public class ProductAttributeDto
{
	public long Id { get; set; }
	public long AttributeId { get; set; }
	public string AttributeName { get; set; } = string.Empty;
	public string AttributeSlug { get; set; } = string.Empty;
	public string? Unit { get; set; }

	public long? AttributeValueId { get; set; }
	public string? Value { get; set; }         // DisplayValue یا CustomValue
	public string? CustomValue { get; set; }

	public int DisplayOrder { get; set; }
}