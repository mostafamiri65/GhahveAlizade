using GhahveAlizade.Application.Features.Seo.Commands.SaveSeoLandingPage;
using GhahveAlizade.Application.Features.Seo.Queries.GetSeoLandingPageById;
using GhahveAlizade.Application.Features.Seo.Queries.GetSeoLandingPagesList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Seo;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class SeoLandingPagesController : AdminBaseController
{
	[HasPermission(Permissions.Seo.ManageLandingPages)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isPublished = null, CancellationToken ct = default)
	{
		ViewData["Title"] = "صفحات فرود سئو";
		ViewData["PageIcon"] = "bi-window-stack";

		var r = await Mediator.Send(new GetSeoLandingPagesListQuery(
			pageNumber, pageSize, search, isPublished), ct);

		if (r.IsError) return Problem(r.Errors);
		ViewBag.Search = search;
		ViewBag.IsPublished = isPublished;
		return View(r.Value);
	}

	[HasPermission(Permissions.Seo.ManageLandingPages)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن صفحه فرود";
		ViewData["PageIcon"] = "bi-plus-circle";
		return View(new SeoLandingPageFormViewModel());
	}

	[HasPermission(Permissions.Seo.ManageLandingPages)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(SeoLandingPageFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new CreateSeoLandingPageCommand(
			model.Title, model.Slug, model.H1, model.Content,
			model.FeaturedImageUrl, model.FilterCriteriaJson, model.IsPublished,
			model.MetaTitle, model.MetaDescription, model.MetaKeywords,
			model.CanonicalUrl, model.OgImage, model.JsonLdSchema), ct);

		if (r.IsError) { AddErrorsToModelState(r.Errors); return View(model); }

		TempData["SuccessMessage"] = "صفحه ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Seo.ManageLandingPages)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش صفحه فرود";
		ViewData["PageIcon"] = "bi-pencil";

		var r = await Mediator.Send(new GetSeoLandingPageByIdQuery(id), ct);
		if (r.IsError) { TempData["ErrorMessage"] = r.FirstError.Description; return RedirectToAction(nameof(Index)); }

		var p = r.Value;
		return View(new SeoLandingPageFormViewModel
		{
			Id = p.Id,
			Title = p.Title,
			Slug = p.Slug,
			H1 = p.H1,
			Content = p.Content,
			FeaturedImageUrl = p.FeaturedImageUrl,
			FilterCriteriaJson = p.FilterCriteriaJson,
			IsPublished = p.IsPublished,
			MetaTitle = p.MetaTitle,
			MetaDescription = p.MetaDescription,
			MetaKeywords = p.MetaKeywords,
			CanonicalUrl = p.CanonicalUrl,
			OgImage = p.OgImage,
			JsonLdSchema = p.JsonLdSchema
		});
	}

	[HasPermission(Permissions.Seo.ManageLandingPages)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, SeoLandingPageFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new UpdateSeoLandingPageCommand(
			id, model.Title, model.Slug, model.H1, model.Content,
			model.FeaturedImageUrl, model.FilterCriteriaJson, model.IsPublished,
			model.MetaTitle, model.MetaDescription, model.MetaKeywords,
			model.CanonicalUrl, model.OgImage, model.JsonLdSchema), ct);

		if (r.IsError) { AddErrorsToModelState(r.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "صفحه ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Seo.ManageLandingPages)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteSeoLandingPageCommand(id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "صفحه حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Seo.ManageLandingPages)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Publish(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new PublishSeoLandingPageCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isPublished = r.Value });
	}
}