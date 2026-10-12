using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace GhahveAlizade.Infrastructure.Seo;

public class SeoHelper : ISeoHelper
{
	private readonly SeoOptions _options;

	public SeoHelper(IOptions<SeoOptions> options) => _options = options.Value;

	public string GetCanonicalUrl(string path)
	{
		var baseUrl = _options.SiteBaseUrl.TrimEnd('/');
		var cleanPath = path.TrimStart('/');
		return $"{baseUrl}/{cleanPath}";
	}

	public string GetAbsoluteUrl(string? relativeUrl)
	{
		if (string.IsNullOrWhiteSpace(relativeUrl)) return _options.SiteBaseUrl;

		if (relativeUrl.StartsWith("http://") || relativeUrl.StartsWith("https://"))
			return relativeUrl;

		var baseUrl = _options.SiteBaseUrl.TrimEnd('/');
		var cleanPath = relativeUrl.TrimStart('/');
		return $"{baseUrl}/{cleanPath}";
	}

	public string GetOgImageUrl(string? imageUrl)
		=> GetAbsoluteUrl(string.IsNullOrWhiteSpace(imageUrl) ? _options.DefaultOgImage : imageUrl);
}