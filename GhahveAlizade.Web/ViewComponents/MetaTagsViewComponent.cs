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

	/// <summary>
	/// seo می‌تواند SeoData باشد یا هر DTO با فیلدهای SEO (مثلاً BlogPostDto / ProductDetailDto).
	/// یک DTO ناهمگون نباید کل صفحه را با InvalidCastException خاموش کند.
	/// </summary>
	public IViewComponentResult Invoke(object? seo = null)
	{
		var httpContext = _httpContextAccessor.HttpContext!;
		var request = httpContext.Request;

		var seoData = seo as SeoData ?? ExtractSeoData(seo);

		var model = new SeoData
		{
			Title = seoData?.Title ?? ViewData["Title"]?.ToString() ?? _options.DefaultMetaTitle,
			Description = seoData?.Description ?? ViewData["Description"]?.ToString() ?? _options.DefaultMetaDescription,
			Keywords = seoData?.Keywords ?? ViewData["Keywords"]?.ToString(),
			CanonicalUrl = seoData?.CanonicalUrl ?? $"{_options.SiteBaseUrl.TrimEnd('/')}{request.Path}",
			OgImage = seoData?.OgImage,
			OgType = seoData?.OgType ?? "website",
			NoIndex = seoData?.NoIndex ?? false,
			NoFollow = seoData?.NoFollow ?? false,
			JsonLd = seoData?.JsonLd
		};

		ViewData["SeoOptions"] = _options;

		return View(model);
	}

	/// <summary>بازتابی و امن: فیلدهای SEO را از هر DTO می‌خواند؛ در بدترین حالت null برمی‌گرداند.</summary>
	private static SeoData? ExtractSeoData(object? dto)
	{
		if (dto is null) return null;

		string? S(string name)
		{
			try
			{
				return dto.GetType().GetProperty(name)?.GetValue(dto) as string;
			}
			catch
			{
				return null;
			}
		}

		var title = S("MetaTitle") ?? S("Title");
		var description = S("MetaDescription") ?? S("Summary") ?? S("ShortDescription");
		if (title is null && description is null) return null;

		return new SeoData
		{
			Title = title,
			Description = description,
			Keywords = S("MetaKeywords"),
			CanonicalUrl = S("CanonicalUrl"),
			OgImage = S("OgImage") ?? S("FeaturedImageUrl") ?? S("PrimaryImageUrl"),
			JsonLd = S("JsonLdSchema") ?? S("JsonLd"),
		};
	}
}