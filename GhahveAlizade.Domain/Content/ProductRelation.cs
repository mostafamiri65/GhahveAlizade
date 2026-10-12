using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Catalog;

public class ProductRelation : BaseEntity
{
	public long ProductId { get; set; }
	public Product Product { get; set; } = default!;

	public long RelatedProductId { get; set; }
	public Product RelatedProduct { get; set; } = default!;

	public string RelationType { get; set; } = default!; // Related, CrossSell, UpSell, Accessory
	public int DisplayOrder { get; set; }
}