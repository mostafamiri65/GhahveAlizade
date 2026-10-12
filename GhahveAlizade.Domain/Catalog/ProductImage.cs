using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Catalog;

public class ProductImage : BaseEntity
{
	public long ProductId { get; set; }
	public Product Product { get; set; } = default!;

	public string ImageUrl { get; set; } = default!;
	public string? ThumbnailUrl { get; set; }
	public string? AltText { get; set; }
	public string? Title { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsPrimary { get; set; }
	public long? FileSizeBytes { get; set; }
	public int? Width { get; set; }
	public int? Height { get; set; }
}