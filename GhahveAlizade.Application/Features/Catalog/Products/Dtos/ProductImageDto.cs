namespace GhahveAlizade.Application.Features.Catalog.Products.Dtos;

public class ProductImageDto
{
	public long Id { get; set; }
	public string ImageUrl { get; set; } = string.Empty;
	public string? ThumbnailUrl { get; set; }
	public string? AltText { get; set; }
	public string? Title { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsPrimary { get; set; }
	public long? FileSizeBytes { get; set; }
	public int? Width { get; set; }
	public int? Height { get; set; }
}