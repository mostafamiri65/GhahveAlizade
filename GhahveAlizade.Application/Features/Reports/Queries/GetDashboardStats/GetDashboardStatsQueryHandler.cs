using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reports.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetDashboardStats;

public class GetDashboardStatsQueryHandler
	: IRequestHandler<GetDashboardStatsQuery, ErrorOr<DashboardStatsDto>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public GetDashboardStatsQueryHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<DashboardStatsDto>> Handle(
		GetDashboardStatsQuery request, CancellationToken ct)
	{
		var now = _dateTime.UtcNow;
		var todayStart = now.Date;
		var yesterdayStart = todayStart.AddDays(-1);
		var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
		var lastMonthStart = monthStart.AddMonths(-1);

		// سفارشات موفق (پرداخت شده و به بعد)
		var paidStatuses = new[]
		{
			OrderStatus.Paid, OrderStatus.Processing, OrderStatus.ReadyToShip,
			OrderStatus.Shipped, OrderStatus.Delivered
		};

		// امروز
		var todayStats = await _context.Orders
			.Where(o => !o.IsDeleted && o.CreatedAtUtc >= todayStart && paidStatuses.Contains(o.Status))
			.GroupBy(o => 1)
			.Select(g => new { Count = g.Count(), Sum = g.Sum(o => o.Total) })
			.FirstOrDefaultAsync(ct);

		// دیروز
		var yesterdayStats = await _context.Orders
			.Where(o => !o.IsDeleted && o.CreatedAtUtc >= yesterdayStart && o.CreatedAtUtc < todayStart && paidStatuses.Contains(o.Status))
			.GroupBy(o => 1)
			.Select(g => new { Count = g.Count(), Sum = g.Sum(o => o.Total) })
			.FirstOrDefaultAsync(ct);

		// این ماه
		var monthStats = await _context.Orders
			.Where(o => !o.IsDeleted && o.CreatedAtUtc >= monthStart && paidStatuses.Contains(o.Status))
			.GroupBy(o => 1)
			.Select(g => new { Count = g.Count(), Sum = g.Sum(o => o.Total) })
			.FirstOrDefaultAsync(ct);

		// ماه قبل (برای مقایسه)
		var lastMonthStats = await _context.Orders
			.Where(o => !o.IsDeleted && o.CreatedAtUtc >= lastMonthStart && o.CreatedAtUtc < monthStart && paidStatuses.Contains(o.Status))
			.GroupBy(o => 1)
			.Select(g => new { Count = g.Count(), Sum = g.Sum(o => o.Total) })
			.FirstOrDefaultAsync(ct);

		// کل
		var totalStats = await _context.Orders
			.Where(o => !o.IsDeleted && paidStatuses.Contains(o.Status))
			.GroupBy(o => 1)
			.Select(g => new { Count = g.Count(), Sum = g.Sum(o => o.Total) })
			.FirstOrDefaultAsync(ct);

		// مشتریان و محصولات
		var totalCustomers = await _context.Customers.CountAsync(c => !c.IsDeleted, ct);
		var totalProducts = await _context.Products.CountAsync(p => !p.IsDeleted, ct);
		var activeProducts = await _context.Products.CountAsync(p => !p.IsDeleted && p.IsActive && p.IsPublished, ct);

		// رشد
		var monthRevenue = monthStats?.Sum ?? 0;
		var lastMonthRevenue = lastMonthStats?.Sum ?? 0;
		var revenueGrowth = lastMonthRevenue > 0
			? Math.Round((monthRevenue - lastMonthRevenue) / lastMonthRevenue * 100, 1)
			: 0;

		var monthOrders = monthStats?.Count ?? 0;
		var lastMonthOrders = lastMonthStats?.Count ?? 0;
		var ordersGrowth = lastMonthOrders > 0
			? Math.Round((decimal)(monthOrders - lastMonthOrders) / lastMonthOrders * 100, 1)
			: 0;

		return new DashboardStatsDto
		{
			TodayOrders = todayStats?.Count ?? 0,
			TodayRevenue = todayStats?.Sum ?? 0,
			YesterdayOrders = yesterdayStats?.Count ?? 0,
			YesterdayRevenue = yesterdayStats?.Sum ?? 0,
			MonthOrders = monthOrders,
			MonthRevenue = monthRevenue,
			TotalOrders = totalStats?.Count ?? 0,
			TotalRevenue = totalStats?.Sum ?? 0,
			TotalCustomers = totalCustomers,
			TotalProducts = totalProducts,
			ActiveProducts = activeProducts,
			RevenueGrowthPercent = revenueGrowth,
			OrdersGrowthPercent = ordersGrowth
		};
	}
}