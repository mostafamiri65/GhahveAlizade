using GhahveAlizade.Application.Features.Customers.Commands.SaveCustomerGroup;
using GhahveAlizade.Application.Features.Customers.Queries.GetCustomerGroupsList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class CustomerGroupsController : AdminBaseController
{
	[HasPermission(Permissions.Customers.ManageGroups)]
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "گروه‌های مشتریان";
		ViewData["PageIcon"] = "bi-people";

		var r = await Mediator.Send(new GetCustomerGroupsListQuery(), ct);
		if (r.IsError) return Problem(r.Errors);

		return View(r.Value);
	}

	[HasPermission(Permissions.Customers.ManageGroups)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		string name, string? description, decimal? discountPercent,
		long? priceListId, bool isActive, int displayOrder, CancellationToken ct)
	{
		var r = await Mediator.Send(new CreateCustomerGroupCommand(
			name, description, discountPercent, priceListId, isActive, displayOrder), ct);

		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "گروه ایجاد شد.", id = r.Value });
	}

	[HasPermission(Permissions.Customers.ManageGroups)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(
		long id, string name, string? description, decimal? discountPercent,
		long? priceListId, bool isActive, int displayOrder, CancellationToken ct)
	{
		var r = await Mediator.Send(new UpdateCustomerGroupCommand(
			id, name, description, discountPercent, priceListId, isActive, displayOrder), ct);

		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "گروه ویرایش شد." });
	}

	[HasPermission(Permissions.Customers.ManageGroups)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteCustomerGroupCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "گروه حذف شد." });
	}
}