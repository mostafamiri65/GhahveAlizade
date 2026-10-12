using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.Catalog;

public class Attribute : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public AttributeType Type { get; set; }
	public string? Unit { get; set; }
	public bool IsFilterable { get; set; }
	public bool IsRequired { get; set; }
	public bool IsVariant { get; set; }
	public int DisplayOrder { get; set; }

	public ICollection<AttributeValue> Values { get; set; } = new List<AttributeValue>();
	public ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();
}