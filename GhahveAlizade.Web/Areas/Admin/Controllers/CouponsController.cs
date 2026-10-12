using GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;
using GhahveAlizade.Application.Features.Promotion.Queries.GetCouponById;
using GhahveAlizade.Application.Features.Promotion.Queries.GetCouponsList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Promotion;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class CouponsController : AdminBaseController
{
	[HasPermission(Permissions.Coupons.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, string? sortBy = "createdAt", string? sortDir = "desc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "کدهای تخفیف";
		ViewData["PageIcon"] = "bi-ticket-perforated";

		var result = await Mediator.Send(new GetCouponsListQuery(
			pageNumber, pageSize, search, isActive, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		return View(result.Value);
	}

	[HasPermission(Permissions.Coupons.Create)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن کد تخفیف";
		ViewData["PageIcon"] = "bi-plus-circle";
		return View(new CouponFormViewModel
		{
			IsActive = true,
			IsPublic = true,
			DiscountType = Domain.Enums.DiscountType.Percentage
		});
	}

	[HasPermission(Permissions.Coupons.Create)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(CouponFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new CreateCouponCommand(
			model.Code, model.Description, model.DiscountType, model.DiscountValue,
			model.MaxDiscountAmount, model.MinOrderAmount, model.MaxUsageCount,
			model.MaxUsagePerCustomer, model.StartsAtUtc, model.ExpiresAtUtc,
			model.IsActive, model.IsFirstOrderOnly, model.IsPublic), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); return View(model); }

		TempData["SuccessMessage"] = "کد تخفیف ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coupons.Edit)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش کد تخفیف";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetCouponByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var c = result.Value;
		return View(new CouponFormViewModel
		{
			Id = c.Id,
			Code = c.Code,
			Description = c.Description,
			DiscountType = c.DiscountType,
			DiscountValue = c.DiscountValue,
			MaxDiscountAmount = c.MaxDiscountAmount,
			MinOrderAmount = c.MinOrderAmount,
			MaxUsageCount = c.MaxUsageCount,
			MaxUsagePerCustomer = c.MaxUsagePerCustomer,
			StartsAtUtc = c.StartsAtUtc,
			ExpiresAtUtc = c.ExpiresAtUtc,
			IsActive = c.IsActive,
			IsFirstOrderOnly = c.IsFirstOrderOnly,
			IsPublic = c.IsPublic
		});
	}

	[HasPermission(Permissions.Coupons.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, CouponFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new UpdateCouponCommand(
			id, model.Code, model.Description, model.DiscountType, model.DiscountValue,
			model.MaxDiscountAmount, model.MinOrderAmount, model.MaxUsageCount,
			model.MaxUsagePerCustomer, model.StartsAtUtc, model.ExpiresAtUtc,
			model.IsActive, model.IsFirstOrderOnly, model.IsPublic), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "کد تخفیف ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coupons.Delete)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteCouponCommand(id), ct);
		TempData[result.IsError ? "ErrorMessage" : "SuccessMessage"] =
			result.IsError ? result.FirstError.Description : "کد تخفیف حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coupons.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ToggleCouponActiveCommand(id), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, isActive = result.Value });
	}
}