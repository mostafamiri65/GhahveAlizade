using GhahveAlizade.Application.Features.Customers.Commands.AddCustomerNote;
using GhahveAlizade.Application.Features.Customers.Commands.DeleteCustomerNote;
using GhahveAlizade.Application.Features.Customers.Commands.SaveCustomerGroup;
using GhahveAlizade.Application.Features.Customers.Commands.ToggleCustomerActive;
using GhahveAlizade.Application.Features.Customers.Commands.UpdateCustomer;
using GhahveAlizade.Application.Features.Customers.Queries.GetCustomerById;
using GhahveAlizade.Application.Features.Customers.Queries.GetCustomerGroupsList;
using GhahveAlizade.Application.Features.Customers.Queries.GetCustomerOrders;
using GhahveAlizade.Application.Features.Customers.Queries.GetCustomersList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class CustomersController : AdminBaseController
{
	// ================ لیست مشتریان ================

	[HasPermission(Permissions.Customers.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		long? customerGroupId = null, bool? isActive = null, bool? isVerified = null,
		bool? hasOrders = null, string? sortBy = "createdAt", string? sortDir = "desc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "مشتریان";
		ViewData["PageIcon"] = "bi-people";

		var r = await Mediator.Send(new GetCustomersListQuery(
			pageNumber, pageSize, search, customerGroupId, isActive, isVerified, hasOrders, sortBy, sortDir), ct);

		if (r.IsError) return Problem(r.Errors);

		var groups = await Mediator.Send(new GetCustomerGroupsListQuery(), ct);
		ViewBag.Groups = groups.IsError ? new() : groups.Value;

		ViewBag.Search = search;
		ViewBag.CustomerGroupId = customerGroupId;
		ViewBag.IsActive = isActive;
		ViewBag.IsVerified = isVerified;
		ViewBag.HasOrders = hasOrders;

		return View(r.Value);
	}

	// ================ جزئیات ================

	[HasPermission(Permissions.Customers.ViewDetails)]
	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات مشتری";
		ViewData["PageIcon"] = "bi-person";

		var r = await Mediator.Send(new GetCustomerByIdQuery(id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var orders = await Mediator.Send(new GetCustomerOrdersQuery(id, 20), ct);
		ViewBag.Orders = orders.IsError ? null : orders.Value;

		var groups = await Mediator.Send(new GetCustomerGroupsListQuery(), ct);
		ViewBag.Groups = groups.IsError ? new() : groups.Value;

		return View(r.Value);
	}

	// ================ ویرایش ================

	[HasPermission(Permissions.Customers.Edit)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش مشتری";
		ViewData["PageIcon"] = "bi-pencil";

		var r = await Mediator.Send(new GetCustomerByIdQuery(id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var groups = await Mediator.Send(new GetCustomerGroupsListQuery(), ct);
		var c = r.Value;

		return View(new CustomerEditViewModel
		{
			Id = c.Id,
			FirstName = c.FirstName,
			LastName = c.LastName,
			Email = c.Email,
			PhoneNumber = c.PhoneNumber,
			NationalCode = c.NationalCode,
			BirthDate = c.BirthDate,
			Gender = c.Gender,
			IsActive = c.IsActive,
			IsVerified = c.IsVerified,
			AcceptsMarketing = c.AcceptsMarketing,
			CustomerGroupId = c.CustomerGroupId,
			Groups = groups.IsError ? new() : groups.Value
		});
	}

	[HasPermission(Permissions.Customers.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, CustomerEditViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid)
		{
			var groups = await Mediator.Send(new GetCustomerGroupsListQuery(), ct);
			model.Groups = groups.IsError ? new() : groups.Value;
			return View(model);
		}

		var r = await Mediator.Send(new UpdateCustomerCommand(
			id, model.FirstName, model.LastName, model.Email, model.PhoneNumber,
			model.NationalCode, model.BirthDate, model.Gender,
			model.IsActive, model.IsVerified, model.AcceptsMarketing,
			model.CustomerGroupId), ct);

		if (r.IsError)
		{
			AddErrorsToModelState(r.Errors);
			var groups = await Mediator.Send(new GetCustomerGroupsListQuery(), ct);
			model.Groups = groups.IsError ? new() : groups.Value;
			return View(model);
		}

		TempData["SuccessMessage"] = "اطلاعات مشتری ذخیره شد.";
		return RedirectToAction(nameof(Detail), new { id });
	}

	// ================ Toggle Active ================

	[HasPermission(Permissions.Customers.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleCustomerActiveCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isActive = r.Value });
	}

	// ================ یادداشت‌ها ================

	[HasPermission(Permissions.Customers.ManageNotes)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> AddNote(
		long customerId, string content, string? category, bool isInternal, CancellationToken ct)
	{
		var r = await Mediator.Send(new AddCustomerNoteCommand(
			customerId, content, category, isInternal), ct);

		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "یادداشت ثبت شد." });
	}

	[HasPermission(Permissions.Customers.ManageNotes)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteNote(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteCustomerNoteCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true });
	}
}