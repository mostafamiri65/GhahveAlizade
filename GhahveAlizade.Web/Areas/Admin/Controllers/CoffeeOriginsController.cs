using GhahveAlizade.Application.Features.Coffee.Commands.CreateCoffeeOrigin;
using GhahveAlizade.Application.Features.Coffee.Commands.DeleteCoffeeOrigin;
using GhahveAlizade.Application.Features.Coffee.Commands.ToggleCoffeeOriginActive;
using GhahveAlizade.Application.Features.Coffee.Commands.UpdateCoffeeOrigin;
using GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeOriginById;
using GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeOriginsList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Coffee;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class CoffeeOriginsController : AdminBaseController
{
	[HasPermission(Permissions.Coffee.ViewOrigins)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, string? sortBy = "displayOrder", string? sortDir = "asc",
		CancellationToken cancellationToken = default)
	{
		ViewData["Title"] = "خاستگاه‌های قهوه";
		ViewData["PageIcon"] = "bi-geo-alt";

		var result = await Mediator.Send(new GetCoffeeOriginsListQuery(
			pageNumber, pageSize, search, isActive, sortBy, sortDir), cancellationToken);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;

		return View(result.Value);
	}

	[HasPermission(Permissions.Coffee.ManageOrigins)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن خاستگاه";
		ViewData["PageIcon"] = "bi-plus-circle";

		return View(new CoffeeOriginFormViewModel
		{
			IsActive = true,
			Regions = new List<CoffeeRegionViewModel>()
		});
	}

	[HasPermission(Permissions.Coffee.ManageOrigins)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(CoffeeOriginFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var command = new CreateCoffeeOriginCommand(
			model.Name, model.Slug, model.Description, model.CountryCode,
			model.FlagUrl, model.ImageUrl, model.IsActive, model.DisplayOrder,
			model.Regions.Where(r => !string.IsNullOrWhiteSpace(r.Name))
				.Select(r => new CoffeeRegionInput(null, r.Name!, r.Slug, r.Description, r.Altitude, r.IsActive, r.DisplayOrder))
				.ToList());

		var result = await Mediator.Send(command, ct);
		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			return View(model);
		}

		TempData["SuccessMessage"] = "خاستگاه با موفقیت ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageOrigins)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش خاستگاه";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetCoffeeOriginByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var o = result.Value;
		var model = new CoffeeOriginFormViewModel
		{
			Id = o.Id,
			Name = o.Name,
			Slug = o.Slug,
			Description = o.Description,
			CountryCode = o.CountryCode,
			FlagUrl = o.FlagUrl,
			ImageUrl = o.ImageUrl,
			IsActive = o.IsActive,
			DisplayOrder = o.DisplayOrder,
			Regions = o.Regions.Select(r => new CoffeeRegionViewModel
			{
				Id = r.Id,
				Name = r.Name,
				Slug = r.Slug,
				Description = r.Description,
				Altitude = r.Altitude,
				IsActive = r.IsActive,
				DisplayOrder = r.DisplayOrder
			}).ToList()
		};

		return View(model);
	}

	[HasPermission(Permissions.Coffee.ManageOrigins)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, CoffeeOriginFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var command = new UpdateCoffeeOriginCommand(
			id, model.Name, model.Slug, model.Description, model.CountryCode,
			model.FlagUrl, model.ImageUrl, model.IsActive, model.DisplayOrder,
			model.Regions.Where(r => !string.IsNullOrWhiteSpace(r.Name))
				.Select(r => new CoffeeRegionInput(
					r.Id > 0 ? r.Id : null, r.Name!, r.Slug, r.Description, r.Altitude, r.IsActive, r.DisplayOrder))
				.ToList());

		var result = await Mediator.Send(command, ct);
		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			model.Id = id;
			return View(model);
		}

		TempData["SuccessMessage"] = "خاستگاه با موفقیت ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageOrigins)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteCoffeeOriginCommand(id), ct);

		if (result.IsError) TempData["ErrorMessage"] = result.FirstError.Description;
		else TempData["SuccessMessage"] = "خاستگاه حذف شد.";

		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageOrigins)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ToggleCoffeeOriginActiveCommand(id), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });

		return Json(new
		{
			success = true,
			isActive = result.Value,
			message = result.Value ? "فعال شد" : "غیرفعال شد"
		});
	}
}