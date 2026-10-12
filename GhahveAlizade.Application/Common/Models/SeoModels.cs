namespace GhahveAlizade.Application.Common.Models;

public class SeoData
{
	public string? Title { get; set; }
	public string? Description { get; set; }
	public string? Keywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }
	public string? OgType { get; set; } = "website";
	public bool NoIndex { get; set; }
	public bool NoFollow { get; set; }
	public string? JsonLd { get; set; }
}

public class SitemapItem
{
	public string Url { get; set; } = default!;
	public DateTime? LastModified { get; set; }
	public string ChangeFrequency { get; set; } = "weekly";
	public decimal Priority { get; set; } = 0.5m;
}