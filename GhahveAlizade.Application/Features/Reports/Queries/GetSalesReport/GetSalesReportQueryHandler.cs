using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reports.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetSalesReport;

public class GetSalesReportQueryHandler
	: IRequestHandler<GetSalesReportQuery, ErrorOr<SalesReportDto>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;
	private readonly IMediator _mediator;

	public GetSalesReportQueryHandler(
		IAppDbContext context,
		IDateTimeProvider dateTime,
		IMediator mediator)
	{
		_context = context;
		_dateTime = dateTime;
		_mediator = mediator;
	}

	public async Task<ErrorOr<SalesReportDto>> Handle(
		GetSalesReportQuery request, CancellationToken ct)
	{
		var fromDate = request.FromDate ?? _dateTime.UtcNow.Date.AddDays(-30);
		var toDate = request.ToDate ?? _dateTime.UtcNow.Date.AddDays(1);

		var paidStatuses = new[]
		{
			OrderStatus.Paid, OrderStatus.Processing, OrderStatus.ReadyToShip,
			OrderStatus.Shipped, OrderStatus.Delivered
		};

		var orders = await _context.Orders
			.Where(o => !o.IsDeleted &&
					   o.CreatedAtUtc >= fromDate &&
					   o.CreatedAtUtc < toDate &&
					   paidStatuses.Contains(o.Status))
			.Include(o => o.Items)
			.ToListAsync(ct);

		var dto = new SalesReportDto
		{
			TotalOrders = orders.Count,
			TotalRevenue = orders.Sum(o => o.Total),
			TotalDiscount = orders.Sum(o => o.DiscountAmount),
			TotalShipping = orders.Sum(o => o.ShippingAmount),
			TotalTax = orders.Sum(o => o.TaxAmount),
			TotalItems = orders.Sum(o => o.Items.Sum(i => i.Quantity)),
			AverageOrderValue = orders.Any()
				? Math.Round(orders.Average(o => o.Total), 0)
				: 0
		};

		// سفارشات بر اساس وضعیت
		var allOrders = await _context.Orders
			.Where(o => !o.IsDeleted && o.CreatedAtUtc >= fromDate && o.CreatedAtUtc < toDate)
			.GroupBy(o => o.Status)
			.Select(g => new { Status = g.Key, Count = g.Count() })
			.ToListAsync(ct);

		foreach (var item in allOrders)
		{
			dto.OrdersByStatus[GetStatusName(item.Status)] = item.Count;
		}

		// درآمد بر اساس دسته‌بندی
		var categoryRevenue = await _context.OrderItems
			.Where(oi => !oi.IsDeleted &&
						oi.Order.CreatedAtUtc >= fromDate &&
						oi.Order.CreatedAtUtc < toDate &&
						!oi.Order.IsDeleted &&
						paidStatuses.Contains(oi.Order.Status))
			.Join(_context.Products,
				oi => oi.ProductId,
				p => p.Id,
				(oi, p) => new { oi, p })
			.Join(_context.Categories,
				x => x.p.CategoryId,
				c => c.Id,
				(x, c) => new { x.oi, CategoryName = c.Name })
			.GroupBy(x => x.CategoryName)
			.Select(g => new
			{
				Category = g.Key ?? "بدون دسته",
				Revenue = g.Sum(x => x.oi.TotalPrice)
			})
			.ToListAsync(ct);

		foreach (var item in categoryRevenue)
		{
			dto.RevenueByCategory[item.Category] = item.Revenue;
		}

		// Top Products
		var topProducts = await _mediator.Send(
			new GetTopProducts.GetTopProductsQuery(10,
				(int)(toDate - fromDate).TotalDays), ct);
		if (!topProducts.IsError) dto.TopProducts = topProducts.Value;

		// Top Customers
		var topCustomers = await _mediator.Send(new GetTopCustomers.GetTopCustomersQuery(10), ct);
		if (!topCustomers.IsError) dto.TopCustomers = topCustomers.Value;

		// Chart
		var chart = await _mediator.Send(
			new GetSalesChart.GetSalesChartQuery((int)(toDate - fromDate).TotalDays), ct);
		if (!chart.IsError) dto.Chart = chart.Value;

		return dto;
	}

	private static string GetStatusName(OrderStatus s) => s switch
	{
		OrderStatus.Pending => "در انتظار",
		OrderStatus.AwaitingPayment => "منتظر پرداخت",
		OrderStatus.Paid => "پرداخت شده",
		OrderStatus.Processing => "در حال پردازش",
		OrderStatus.ReadyToShip => "آماده ارسال",
		OrderStatus.Shipped => "ارسال شده",
		OrderStatus.Delivered => "تحویل شده",
		OrderStatus.Cancelled => "لغو شده",
		OrderStatus.Refunded => "مرجوع شده",
		OrderStatus.Returned => "بازگشت",
		_ => s.ToString()
	};
}