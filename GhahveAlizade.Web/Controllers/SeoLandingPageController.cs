using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Seo.Queries.GetSeoLandingPageBySlug;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Controllers;

public class SeoLandingPageController : MvcBaseController
{
	private readonly ISchemaBuilder _schemaBuilder;

	public SeoLandingPageController(ISchemaBuilder schemaBuilder)
		=> _schemaBuilder = schemaBuilder;

	[HttpGet("/{slug:regex(^[[a-z0-9\\-]]+$)}")]
	public async Task<IActionResult> Show(string slug, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(
			new GetSeoLandingPageBySlugQuery(slug), cancellationToken);

		if (result.IsError)
			return NotFound();

		var page = result.Value;

		ViewData["Title"] = page.MetaTitle ?? page.Title;
		ViewData["Description"] = page.MetaDescription;

		ViewBag.JsonLd = _schemaBuilder.Serialize(
			_schemaBuilder.BuildArticle(
				page.Title,
				page.MetaDescription,
				page.FeaturedImageUrl,
				page.PublishedAtUtc ?? DateTime.UtcNow,
				null,
				null,
				$"/{page.Slug}"));

		return View(page);
	}
}