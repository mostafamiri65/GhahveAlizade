using GhahveAlizade.Application.Features.UserPanel.Commands.DeleteMyAddress;
using GhahveAlizade.Application.Features.UserPanel.Commands.SaveMyAddress;
using GhahveAlizade.Application.Features.UserPanel.Queries.GetMyAddresses;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

public class AddressesController : UserBaseController
{
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "آدرس‌های من";
		var r = await Mediator.Send(new GetMyAddressesQuery(CurrentUserId), ct);
		if (r.IsError) return Problem(r.Errors);
		return View(r.Value);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Save(AddressInputModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid)
		{
			TempData["ErrorMessage"] = "اطلاعات فرم نامعتبر است.";
			return RedirectToAction(nameof(Index));
		}

		var r = await Mediator.Send(new SaveMyAddressCommand(
			CurrentUserId, model.Id, model.Title, model.FirstName, model.LastName,
			model.PhoneNumber, model.Province, model.City, model.PostalCode,
			model.AddressLine1, model.AddressLine2, model.IsDefault), ct);

		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "آدرس ذخیره شد.";
		return RedirectToAction(nameof(Index));
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteMyAddressCommand(CurrentUserId, id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "آدرس حذف شد.";
		return RedirectToAction(nameof(Index));
	}
}

public class AddressInputModel
{
	public long? Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;
	public string Province { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string AddressLine1 { get; set; } = string.Empty;
	public string? AddressLine2 { get; set; }
	public bool IsDefault { get; set; }
}
