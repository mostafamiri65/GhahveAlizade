using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GhahveAlizade.Web.Controllers;

[Route("robots.txt")]
[ResponseCache(Duration = 86400)] // 1 روز
public class RobotsController : Controller
{
	private readonly ISeoHelper _seoHelper;
	private readonly IWebHostEnvironment _env;

	public RobotsController(ISeoHelper seoHelper, IWebHostEnvironment env)
	{
		_seoHelper = seoHelper;
		_env = env;
	}

	[HttpGet("")]
	public IActionResult Get()
	{
		var sb = new StringBuilder();

		// اگر در Development هستیم، همه چیز رو بلاک کن
		if (_env.IsDevelopment())
		{
			sb.AppendLine("User-agent: *");
			sb.AppendLine("Disallow: /");
			return Content(sb.ToString(), "text/plain", Encoding.UTF8);
		}

		sb.AppendLine("User-agent: *");
		sb.AppendLine("Allow: /");

		// بلاک کردن مسیرهای خصوصی
		sb.AppendLine("Disallow: /admin/");
		sb.AppendLine("Disallow: /account/");
		sb.AppendLine("Disallow: /identity/");
		sb.AppendLine("Disallow: /api/");
		sb.AppendLine("Disallow: /cart/");
		sb.AppendLine("Disallow: /checkout/");
		sb.AppendLine("Disallow: /order/");
		sb.AppendLine("Disallow: /user/");
		sb.AppendLine("Disallow: /hangfire/");
		sb.AppendLine("Disallow: /health");
		sb.AppendLine("Disallow: /*?*sort=");
		sb.AppendLine("Disallow: /*?*page=");
		sb.AppendLine("Disallow: /*?*filter=");
		sb.AppendLine();

		// Sitemaps
		sb.AppendLine($"Sitemap: {_seoHelper.GetCanonicalUrl("sitemap.xml")}");
		sb.AppendLine();

		// Crawl delay (اختیاری)
		sb.AppendLine("Crawl-delay: 1");

		return Content(sb.ToString(), "text/plain", Encoding.UTF8);
	}
}