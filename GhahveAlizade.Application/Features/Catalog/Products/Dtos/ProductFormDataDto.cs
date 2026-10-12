namespace GhahveAlizade.Application.Features.Catalog.Products.Dtos;

public class ProductFormDataDto
{
	public List<LookupItem> Categories { get; set; } = new();
	public List<LookupItem> Brands { get; set; } = new();
	public List<LookupItem> CoffeeOrigins { get; set; } = new();
	public List<LookupItem> CoffeeProcesses { get; set; } = new();
	public List<LookupItem> RoastProfiles { get; set; } = new();
	public List<LookupItem> FlavorNotes { get; set; } = new();
	public List<LookupItem> Attributes { get; set; } = new();
}

public class LookupItem
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Extra { get; set; }   // مثلاً ColorCode یا CountryCode
}