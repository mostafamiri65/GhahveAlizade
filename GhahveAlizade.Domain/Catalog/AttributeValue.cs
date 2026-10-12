using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Catalog;

public class AttributeValue : BaseEntity
{
	public long AttributeId { get; set; }
	public Attribute Attribute { get; set; } = default!;

	public string Value { get; set; } = default!;
	public string? DisplayValue { get; set; }
	public string? ColorCode { get; set; }
	public int DisplayOrder { get; set; }
}