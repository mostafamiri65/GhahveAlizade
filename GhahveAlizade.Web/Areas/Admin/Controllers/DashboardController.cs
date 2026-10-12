using GhahveAlizade.Application.Features.Reports.Queries.GetDashboardStats;
using GhahveAlizade.Application.Features.Reports.Queries.GetLowStockAlerts;
using GhahveAlizade.Application.Features.Reports.Queries.GetRecentOrders;
using GhahveAlizade.Application.Features.Reports.Queries.GetSalesChart;
using GhahveAlizade.Application.Features.Reports.Queries.GetTopProducts;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class DashboardController : AdminBaseController
{
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "داشبورد";
		ViewData["PageIcon"] = "bi-speedometer2";

		// Stats
		var stats = await Mediator.Send(new GetDashboardStatsQuery(), ct);
		ViewBag.Stats = stats.IsError ? null : stats.Value;

		// Chart
		var chart = await Mediator.Send(new GetSalesChartQuery(30), ct);
		ViewBag.Chart = chart.IsError ? null : chart.Value;

		// Top products
		var topProducts = await Mediator.Send(new GetTopProductsQuery(5, 30), ct);
		ViewBag.TopProducts = topProducts.IsError ? null : topProducts.Value;

		// Recent orders
		var recentOrders = await Mediator.Send(new GetRecentOrdersQuery(8), ct);
		ViewBag.RecentOrders = recentOrders.IsError ? null : recentOrders.Value;

		// Low stock
		var lowStock = await Mediator.Send(new GetLowStockAlertsQuery(5), ct);
		ViewBag.LowStock = lowStock.IsError ? null : lowStock.Value;

		return View();
	}
}