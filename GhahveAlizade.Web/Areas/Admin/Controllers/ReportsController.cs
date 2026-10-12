using System.Text;
using GhahveAlizade.Application.Features.Reports.Queries.GetInventoryReport;
using GhahveAlizade.Application.Features.Reports.Queries.GetSalesReport;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class ReportsController : AdminBaseController
{
	// ================ گزارش فروش ================

	[HasPermission(Permissions.Reports.ViewSales)]
	public async Task<IActionResult> Sales(
		DateTime? fromDate = null, DateTime? toDate = null,
		CancellationToken ct = default)
	{
		ViewData["Title"] = "گزارش فروش";
		ViewData["PageIcon"] = "bi-graph-up";

		var r = await Mediator.Send(new GetSalesReportQuery(fromDate, toDate), ct);
		if (r.IsError) return Problem(r.Errors);

		ViewBag.FromDate = fromDate;
		ViewBag.ToDate = toDate;

		return View(r.Value);
	}

	// ================ گزارش انبار ================

	[HasPermission(Permissions.Reports.ViewInventory)]
	public async Task<IActionResult> Inventory(CancellationToken ct)
	{
		ViewData["Title"] = "گزارش انبار";
		ViewData["PageIcon"] = "bi-clipboard-data";

		var r = await Mediator.Send(new GetInventoryReportQuery(), ct);
		if (r.IsError) return Problem(r.Errors);

		return View(r.Value);
	}

	// ================ خروجی CSV ================

	[HasPermission(Permissions.Reports.Export)]
	[HttpGet]
	public async Task<IActionResult> ExportSales(
		DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
	{
		var r = await Mediator.Send(new GetSalesReportQuery(fromDate, toDate), ct);
		if (r.IsError) return Problem(r.Errors);

		var sb = new StringBuilder();
		sb.AppendLine("تاریخ,سفارشات,درآمد (تومان)");

		foreach (var p in r.Value.Chart)
		{
			sb.AppendLine($"{p.Date},{p.Orders},{p.Revenue}");
		}

		sb.AppendLine();
		sb.AppendLine("جمع کل");
		sb.AppendLine($"تعداد سفارشات,{r.Value.TotalOrders}");
		sb.AppendLine($"درآمد,{r.Value.TotalRevenue}");
		sb.AppendLine($"میانگین سفارش,{r.Value.AverageOrderValue}");

		var bytes = Encoding.UTF8.GetPreamble()
			.Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();

		return File(bytes, "text/csv", $"sales-report-{DateTime.UtcNow:yyyyMMdd}.csv");
	}
}