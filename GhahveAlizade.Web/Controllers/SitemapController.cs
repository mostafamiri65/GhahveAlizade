using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml.Linq;

namespace GhahveAlizade.Web.Controllers;

[Route("sitemap")]
[ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)] // 1 ساعت کش
public class SitemapController : Controller
{
	private readonly IAppDbContext _context;
	private readonly ISeoHelper _seoHelper;
	private readonly ILogger<SitemapController> _logger;

	public SitemapController(
		IAppDbContext context,
		ISeoHelper seoHelper,
		ILogger<SitemapController> logger)
	{
		_context = context;
		_seoHelper = seoHelper;
		_logger = logger;
	}

	[HttpGet("")]
	[HttpGet("index.xml")]
	[Produces("application/xml")]
	public async Task<IActionResult> Index(CancellationToken cancellationToken)
	{
		var sitemaps = new[]
		{
			new { Url = _seoHelper.GetCanonicalUrl("sitemap/products.xml"), LastMod = DateTime.UtcNow },
			new { Url = _seoHelper.GetCanonicalUrl("sitemap/categories.xml"), LastMod = DateTime.UtcNow },
			new { Url = _seoHelper.GetCanonicalUrl("sitemap/brands.xml"), LastMod = DateTime.UtcNow },
			new { Url = _seoHelper.GetCanonicalUrl("sitemap/blog.xml"), LastMod = DateTime.UtcNow },
			new { Url = _seoHelper.GetCanonicalUrl("sitemap/pages.xml"), LastMod = DateTime.UtcNow }
		};

		XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

		var doc = new XDocument(
			new XDeclaration("1.0", "utf-8", "yes"),
			new XElement(ns + "sitemapindex",
				sitemaps.Select(s => new XElement(ns + "sitemap",
					new XElement(ns + "loc", s.Url),
					new XElement(ns + "lastmod", s.LastMod.ToString("yyyy-MM-dd"))))));

		return Content(doc.ToString(), "application/xml", Encoding.UTF8);
	}

	[HttpGet("products.xml")]
	[Produces("application/xml")]
	public async Task<IActionResult> Products(CancellationToken cancellationToken)
	{
		var products = await _context.Products
			.AsNoTracking()
			.Where(p => !p.IsDeleted && p.IsActive && p.IsPublished)
			.OrderByDescending(p => p.UpdatedAtUtc ?? p.CreatedAtUtc)
			.Select(p => new
			{
				p.Slug,
				LastMod = p.UpdatedAtUtc ?? p.CreatedAtUtc,
				p.IsFeatured
			})
			.Take(50000) // حد مجاز sitemap
			.ToListAsync(cancellationToken);

		var items = products.Select(p => new SitemapItem
		{
			Url = _seoHelper.GetCanonicalUrl($"product/{p.Slug}"),
			LastModified = p.LastMod,
			ChangeFrequency = p.IsFeatured ? "daily" : "weekly",
			Priority = p.IsFeatured ? 0.9m : 0.7m
		}).ToList();

		return Xml(BuildSitemap(items));
	}

	[HttpGet("categories.xml")]
	[Produces("application/xml")]
	public async Task<IActionResult> Categories(CancellationToken cancellationToken)
	{
		var categories = await _context.Categories
			.AsNoTracking()
			.Where(c => !c.IsDeleted && c.IsActive)
			.Select(c => new
			{
				c.Slug,
				LastMod = c.UpdatedAtUtc ?? c.CreatedAtUtc,
				c.IsFeatured
			})
			.ToListAsync(cancellationToken);

		var items = categories.Select(c => new SitemapItem
		{
			Url = _seoHelper.GetCanonicalUrl($"category/{c.Slug}"),
			LastModified = c.LastMod,
			ChangeFrequency = "weekly",
			Priority = c.IsFeatured ? 0.8m : 0.6m
		}).ToList();

		return Xml(BuildSitemap(items));
	}

	[HttpGet("brands.xml")]
	[Produces("application/xml")]
	public async Task<IActionResult> Brands(CancellationToken cancellationToken)
	{
		var brands = await _context.Brands
			.AsNoTracking()
			.Where(b => !b.IsDeleted && b.IsActive)
			.Select(b => new
			{
				b.Slug,
				LastMod = b.UpdatedAtUtc ?? b.CreatedAtUtc
			})
			.ToListAsync(cancellationToken);

		var items = brands.Select(b => new SitemapItem
		{
			Url = _seoHelper.GetCanonicalUrl($"brand/{b.Slug}"),
			LastModified = b.LastMod,
			ChangeFrequency = "weekly",
			Priority = 0.6m
		}).ToList();

		return Xml(BuildSitemap(items));
	}

	[HttpGet("blog.xml")]
	[Produces("application/xml")]
	public async Task<IActionResult> Blog(CancellationToken cancellationToken)
	{
		var posts = await _context.BlogPosts
			.AsNoTracking()
			.Where(p => !p.IsDeleted && p.IsPublished)
			.OrderByDescending(p => p.PublishedAtUtc)
			.Select(p => new
			{
				p.Slug,
				LastMod = p.UpdatedAtUtc ?? p.PublishedAtUtc ?? p.CreatedAtUtc,
				p.IsFeatured
			})
			.ToListAsync(cancellationToken);

		var items = posts.Select(p => new SitemapItem
		{
			Url = _seoHelper.GetCanonicalUrl($"blog/{p.Slug}"),
			LastModified = p.LastMod,
			ChangeFrequency = "monthly",
			Priority = p.IsFeatured ? 0.8m : 0.6m
		}).ToList();

		return Xml(BuildSitemap(items));
	}

	[HttpGet("pages.xml")]
	[Produces("application/xml")]
	public async Task<IActionResult> Pages(CancellationToken cancellationToken)
	{
		var items = new List<SitemapItem>
		{
			new() { Url = _seoHelper.GetCanonicalUrl(""), LastModified = DateTime.UtcNow, ChangeFrequency = "daily", Priority = 1.0m },
			new() { Url = _seoHelper.GetCanonicalUrl("about"), LastModified = DateTime.UtcNow, ChangeFrequency = "monthly", Priority = 0.5m },
			new() { Url = _seoHelper.GetCanonicalUrl("contact"), LastModified = DateTime.UtcNow, ChangeFrequency = "monthly", Priority = 0.5m },
			new() { Url = _seoHelper.GetCanonicalUrl("blog"), LastModified = DateTime.UtcNow, ChangeFrequency = "daily", Priority = 0.7m },
			new() { Url = _seoHelper.GetCanonicalUrl("products"), LastModified = DateTime.UtcNow, ChangeFrequency = "daily", Priority = 0.9m }
		};

		// صفحات فرود سئو
		var landingPages = await _context.SeoLandingPages
			.AsNoTracking()
			.Where(l => !l.IsDeleted && l.IsPublished)
			.Select(l => new { l.Slug, l.UpdatedAtUtc })
			.ToListAsync(cancellationToken);

		items.AddRange(landingPages.Select(l => new SitemapItem
		{
			Url = _seoHelper.GetCanonicalUrl(l.Slug),
			LastModified = l.UpdatedAtUtc ?? DateTime.UtcNow,
			ChangeFrequency = "weekly",
			Priority = 0.7m
		}));

		return Xml(BuildSitemap(items));
	}

	private ContentResult Xml(string content)
		=> Content(content, "application/xml", Encoding.UTF8);

	private string BuildSitemap(List<SitemapItem> items)
	{
		XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
		XNamespace xhtml = "http://www.w3.org/1999/xhtml";

		var doc = new XDocument(
			new XDeclaration("1.0", "utf-8", "yes"),
			new XElement(ns + "urlset",
				new XAttribute("xmlns", ns.NamespaceName),
				new XAttribute(XNamespace.Xmlns + "xhtml", xhtml.NamespaceName),
				items.Select(item =>
					new XElement(ns + "url",
						new XElement(ns + "loc", item.Url),
						item.LastModified.HasValue
							? new XElement(ns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd"))
							: null,
						new XElement(ns + "changefreq", item.ChangeFrequency),
						new XElement(ns + "priority", item.Priority.ToString("0.0",
							System.Globalization.CultureInfo.InvariantCulture))
					))));

		return doc.ToString();
	}
}