using GhahveAlizade.Application.Features.Seo.Commands.SaveSeoMetadata;
using GhahveAlizade.Application.Features.Seo.Queries.GetSeoMetadataById;
using GhahveAlizade.Application.Features.Seo.Queries.GetSeoMetadataList;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Seo;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class SeoController : AdminBaseController
{
	[HasPermission(Permissions.Seo.ViewMetadata)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		SeoEntityType? entityType = null, CancellationToken ct = default)
	{
		ViewData["Title"] = "متادیتای سئو";
		ViewData["PageIcon"] = "bi-search";

		var r = await Mediator.Send(new GetSeoMetadataListQuery(
			pageNumber, pageSize, search, entityType), ct);

		if (r.IsError) return Problem(r.Errors);

		ViewBag.Search = search;
		ViewBag.EntityType = entityType;
		return View(r.Value);
	}

	[HasPermission(Permissions.Seo.EditMetadata)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش متادیتای سئو";
		ViewData["PageIcon"] = "bi-search";

		var r = await Mediator.Send(new GetSeoMetadataByIdQuery(id), ct);
		if (r.IsError) { TempData["ErrorMessage"] = r.FirstError.Description; return RedirectToAction(nameof(Index)); }

		var s = r.Value;
		return View(new SeoMetadataFormViewModel
		{
			Id = s.Id,
			EntityType = s.EntityType,
			EntityId = s.EntityId,
			Culture = s.Culture,
			MetaTitle = s.MetaTitle,
			MetaDescription = s.MetaDescription,
			MetaKeywords = s.MetaKeywords,
			CanonicalUrl = s.CanonicalUrl,
			OgTitle = s.OgTitle,
			OgDescription = s.OgDescription,
			OgImage = s.OgImage,
			OgType = s.OgType,
			TwitterCard = s.TwitterCard,
			TwitterTitle = s.TwitterTitle,
			TwitterDescription = s.TwitterDescription,
			TwitterImage = s.TwitterImage,
			Robots = s.Robots,
			JsonLdSchema = s.JsonLdSchema,
			FocusKeyword = s.FocusKeyword,
			SeoScore = s.SeoScore
		});
	}

	[HasPermission(Permissions.Seo.EditMetadata)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, SeoMetadataFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new UpsertSeoMetadataCommand(
			id, model.EntityType, model.EntityId, model.Culture,
			model.MetaTitle, model.MetaDescription, model.MetaKeywords, model.CanonicalUrl,
			model.OgTitle, model.OgDescription, model.OgImage, model.OgType,
			model.TwitterCard, model.TwitterTitle, model.TwitterDescription, model.TwitterImage,
			model.Robots, model.JsonLdSchema, model.FocusKeyword, model.SeoScore), ct);

		if (r.IsError) { AddErrorsToModelState(r.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "متادیتا ذخیره شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Seo.EditMetadata)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteSeoMetadataCommand(id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "متادیتا حذف شد.";
		return RedirectToAction(nameof(Index));
	}
}