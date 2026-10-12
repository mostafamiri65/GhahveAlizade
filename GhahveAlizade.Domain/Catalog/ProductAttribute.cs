using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Catalog;

public class ProductAttribute : BaseEntity
{
	public long ProductId { get; set; }
	public Product Product { get; set; } = default!;

	public long AttributeId { get; set; }
	public Attribute Attribute { get; set; } = default!;

	public long? AttributeValueId { get; set; }
	public AttributeValue? AttributeValue { get; set; }

	public string? CustomValue { get; set; }

	public int DisplayOrder { get; set; }
}