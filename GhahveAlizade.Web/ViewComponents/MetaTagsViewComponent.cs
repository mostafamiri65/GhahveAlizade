using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Infrastructure.Seo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GhahveAlizade.Web.ViewComponents;

public class MetaTagsViewComponent : ViewComponent
{
	private readonly SeoOptions _options;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public MetaTagsViewComponent(
		IOptions<SeoOptions> options,
		IHttpContextAccessor httpContextAccessor)
	{
		_options = options.Value;
		_httpContextAccessor = httpContextAccessor;
	}

	public IViewComponentResult Invoke(SeoData? seo = null)
	{
		var httpContext = _httpContextAccessor.HttpContext!;
		var request = httpContext.Request;

		var model = new SeoData
		{
			Title = seo?.Title ?? ViewData["Title"]?.ToString() ?? _options.DefaultMetaTitle,
			Description = seo?.Description ?? ViewData["Description"]?.ToString() ?? _options.DefaultMetaDescription,
			Keywords = seo?.Keywords ?? ViewData["Keywords"]?.ToString(),
			CanonicalUrl = seo?.CanonicalUrl ?? $"{_options.SiteBaseUrl.TrimEnd('/')}{request.Path}",
			OgImage = seo?.OgImage,
			OgType = seo?.OgType ?? "website",
			NoIndex = seo?.NoIndex ?? false,
			NoFollow = seo?.NoFollow ?? false,
			JsonLd = seo?.JsonLd
		};

		ViewData["SeoOptions"] = _options;

		return View(model);
	}
}